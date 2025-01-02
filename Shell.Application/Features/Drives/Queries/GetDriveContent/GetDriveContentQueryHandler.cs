using MediatR;
using Microsoft.EntityFrameworkCore;
using Shell.Application.Common.Exceptions;
using Shell.Application.Common.Interfaces;
using Shell.Application.Features.Directories.Commands.CreateDirectory;
using Shell.Application.Features.Directories.Commands.DeleteDirectory;
using Shell.Application.Features.Files.Commands.CreateFile;
using Shell.Application.Features.Files.Commands.DeleteFile;
using Shell.Domain.Entities;

namespace Shell.Application.Features.Drives.Queries.GetDriveContent
{
    public class GetDriveContentQueryHandler(IShellDbContext dbContext, IMediator mediator)
        : IRequestHandler<GetDriveContentQuery, GetDriveContentQueryResponse>
    {
        public async Task<GetDriveContentQueryResponse> Handle(GetDriveContentQuery request, CancellationToken cancellationToken)
        {
            var drive = await dbContext.Drives
                .Include(drive => drive.Directories)
                .Include(drive => drive.Files)
                .SingleOrDefaultAsync(drive => drive.Name.Equals(request.Name), cancellationToken)
                ?? throw new NotFoundException(nameof(Drive), request.Name);

            // Add if only local
            foreach (var directoryPath in System.IO.Directory.GetDirectories(drive.Name))
            {
                var directoryExist = await dbContext.Directories
                    .AnyAsync(directory => directory.Path.Equals(directoryPath), cancellationToken);

                if (!directoryExist)
                    await mediator.Send(new CreateDirectoryCommand { Path = directoryPath }, cancellationToken);
            }

            foreach (var filePath in System.IO.Directory.GetFiles(drive.Name))
            {
                var fileExist = await dbContext.Files
                    .AnyAsync(file => file.Path.Equals(filePath), cancellationToken);

                if (!fileExist)
                    await mediator.Send(new CreateFileCommand { Path = filePath }, cancellationToken);
            }

            // Delete if only in db
            var driveDirectories = await dbContext.Directories
                .Where(directory => directory.ParentDirectory == null && directory.Drive.Name.Equals(drive.Name))
                .ToListAsync(cancellationToken);

            foreach (var directory in driveDirectories.Where(directory => !System.IO.Directory.Exists(directory.Path)))
            {
                await mediator.Send(new DeleteDirectoryCommand { Path = directory.Path }, cancellationToken);
            }

            var driveFiles = await dbContext.Files
                .Where(file => file.Directory == null && file.Drive.Name.Equals(drive.Name))
                .ToListAsync(cancellationToken);

            foreach (var file in driveFiles.Where(file => !System.IO.File.Exists(file.Path)))
            {
                await mediator.Send(new DeleteFileCommand { Path = file.Path }, cancellationToken);
            }

            // Get content
            var directories = drive.Directories
                .Select(directory => new DriveItemDto
                {
                    Name = directory.Name,
                    Path = directory.Path,
                    CreationDate = directory.CreationDate,
                    EditDate = directory.EditDate,
                    IsDirectory = true,
                });

            var files = drive.Files
                .Select(file => new DriveItemDto
                {
                    Name = file.Name,
                    Path = file.Path,
                    CreationDate = file.CreationDate,
                    EditDate = file.EditDate,
                    IsDirectory = false,
                });

            var content = directories.Concat(files).ToList();

            return new GetDriveContentQueryResponse { Content = content };
        }
    }
}
