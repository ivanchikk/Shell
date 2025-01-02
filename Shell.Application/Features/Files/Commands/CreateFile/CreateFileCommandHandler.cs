using MediatR;
using Microsoft.EntityFrameworkCore;
using Shell.Application.Common.Exceptions;
using Shell.Application.Common.Interfaces;
using Shell.Application.Features.Directories.Commands.CreateDirectory;
using Shell.Application.Features.Directories.Commands.DeleteDirectory;
using Shell.Application.Features.Files.Commands.DeleteFile;
using Shell.Domain.Entities;
using Directory = Shell.Domain.Entities.Directory;
using File = Shell.Domain.Entities.File;

namespace Shell.Application.Features.Files.Commands.CreateFile
{
    public class CreateFileCommandHandler(
        IShellDbContext dbContext,
        IFileSystemService fileSystemService,
        IMediator mediator)
        : IRequestHandler<CreateFileCommand, string>
    {
        public async Task<string> Handle(CreateFileCommand request, CancellationToken cancellationToken)
        {
            var existInDb = await dbContext.Files
                .AnyAsync(file => file.Path.Equals(request.Path), cancellationToken);
            var existLocal = System.IO.File.Exists(request.Path);

            if (existInDb)
            {
                if (existLocal)
                    throw new DuplicateException(nameof(File), request.Path);
                await mediator.Send(new DeleteFileCommand { Path = request.Path }, cancellationToken);
            }

            var fileDrivePath = Path.GetPathRoot(request.Path)!;
            var fileDrive = await dbContext.Drives
                .SingleOrDefaultAsync(drive => drive.Name.Equals(fileDrivePath), cancellationToken)
                ?? throw new NotFoundException(nameof(Drive), fileDrivePath);

            var fileDirectoryPath = Path.GetDirectoryName(request.Path);
            var fileDirectory = await dbContext.Directories
                .SingleOrDefaultAsync(directory => directory.Path.Equals(fileDirectoryPath), cancellationToken);
            var directories = request.Path.Split(Path.DirectorySeparatorChar).Length > 2;
            var fileDirectoryExistsLocal = System.IO.Directory.Exists(fileDirectoryPath);

            if (directories && !fileDirectoryExistsLocal)
            {
                if (fileDirectory != null)
                    await mediator.Send(new DeleteDirectoryCommand { Path = fileDirectoryPath! }, cancellationToken);
                throw new NotFoundException(nameof(Directory), fileDirectoryPath!);
            }

            if (fileDirectory == null && directories && fileDirectoryExistsLocal)
            {
                await mediator.Send(new CreateDirectoryCommand { Path = fileDirectoryPath! }, cancellationToken);

                fileDirectory = await dbContext.Directories
                    .SingleOrDefaultAsync(directory => directory.Path.Equals(fileDirectoryPath), cancellationToken);
            }

            var file = new File
            {
                Name = Path.GetFileName(request.Path),
                Path = request.Path,
                CreationDate = DateTime.UtcNow,
                EditDate = DateTime.UtcNow,
                Directory = fileDirectory,
                DirectoryId = fileDirectory?.Id,
                Drive = fileDrive,
                DriveId = fileDrive.Id,
            };

            // Create local
            if (!existLocal)
            {
                try
                {
                    fileSystemService.CreateFile(request.Path);
                }
                catch (Exception e)
                {
                    throw new CreateException(nameof(File), request.Path, e);
                }
            }

            // Save in db
            await dbContext.Files.AddAsync(file, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return request.Path;
        }
    }
}
