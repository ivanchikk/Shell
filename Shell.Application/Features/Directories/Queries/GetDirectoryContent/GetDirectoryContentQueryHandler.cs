using MediatR;
using Microsoft.EntityFrameworkCore;
using Shell.Application.Common.Exceptions;
using Shell.Application.Common.Interfaces;
using Shell.Application.Features.Directories.Commands.CreateDirectory;
using Shell.Application.Features.Directories.Commands.DeleteDirectory;
using Shell.Application.Features.Files.Commands.CreateFile;
using Shell.Application.Features.Files.Commands.DeleteFile;
using Directory = Shell.Domain.Entities.Directory;

namespace Shell.Application.Features.Directories.Queries.GetDirectoryContent
{
    public class GetDirectoryContentQueryHandler(IShellDbContext dbContext, IMediator mediator)
        : IRequestHandler<GetDirectoryContentQuery, GetDirectoryContentQueryResponse>
    {
        public async Task<GetDirectoryContentQueryResponse> Handle(GetDirectoryContentQuery request, CancellationToken cancellationToken)
        {
            var directory = await dbContext.Directories
                .Include(directory => directory.ChildDirectories)
                .Include(directory => directory.Files)
                .SingleOrDefaultAsync(directory => directory.Path.Equals(request.Path), cancellationToken);

            if (!System.IO.Directory.Exists(request.Path))
            {
                if (directory != null)
                    await mediator.Send(new DeleteDirectoryCommand { Path = request.Path }, cancellationToken);
                throw new NotFoundException(nameof(Directory), request.Path);
            }
            else if (directory == null)
            {
                await mediator.Send(new CreateDirectoryCommand { Path = request.Path }, cancellationToken);

                directory = await dbContext.Directories
                    .Include(d => d.ChildDirectories)
                    .Include(d => d.Files)
                    .SingleOrDefaultAsync(d => d.Path.Equals(request.Path), cancellationToken);
            }

            // Add if only local
            foreach (var childDirectoryPath in System.IO.Directory.GetDirectories(request.Path))
            {
                var childDirectoryExist = await dbContext.Directories
                    .AnyAsync(d => d.Path.Equals(childDirectoryPath), cancellationToken);

                if (!childDirectoryExist)
                    await mediator.Send(new CreateDirectoryCommand { Path = childDirectoryPath }, cancellationToken);
            }

            foreach (var filePath in System.IO.Directory.GetFiles(request.Path))
            {
                var fileExist = await dbContext.Files
                    .AnyAsync(file => file.Path.Equals(filePath), cancellationToken);

                if (!fileExist)
                    await mediator.Send(new CreateFileCommand { Path = filePath }, cancellationToken);
            }

            // Delete if only in db
            var directoryChildDirectories = await dbContext.Directories
                .Where(d => d.ParentDirectory != null && d.ParentDirectory.Path.Equals(request.Path))
                .ToListAsync(cancellationToken);

            foreach (var childDirectory in directoryChildDirectories.Where(childDirectory => !System.IO.Directory.Exists(childDirectory.Path)))
            {
                await mediator.Send(new DeleteDirectoryCommand { Path = childDirectory.Path }, cancellationToken);
            }

            var directoryFiles = await dbContext.Files
                .Where(file => file.Directory != null && file.Directory.Path.Equals(request.Path))
                .ToListAsync(cancellationToken);

            foreach (var file in directoryFiles.Where(file => !System.IO.File.Exists(file.Path)))
            {
                await mediator.Send(new DeleteFileCommand { Path = file.Path }, cancellationToken);
            }

            // Get content
            var childDirectories = directory!.ChildDirectories
                .Select(childDirectory => new DirectoryItemDto
                {
                    Name = childDirectory.Name,
                    Path = childDirectory.Path,
                    CreationDate = childDirectory.CreationDate,
                    EditDate = childDirectory.EditDate,
                    IsDirectory = true,
                });

            var files = directory.Files
                .Select(file => new DirectoryItemDto
                {
                    Name = file.Name,
                    Path = file.Path,
                    CreationDate = file.CreationDate,
                    EditDate = file.EditDate,
                    IsDirectory = false,
                });

            var content = childDirectories.Concat(files).ToList();

            return new GetDirectoryContentQueryResponse { Content = content };
        }
    }
}
