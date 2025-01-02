using MediatR;
using Microsoft.EntityFrameworkCore;
using Shell.Application.Common.Exceptions;
using Shell.Application.Common.Interfaces;
using Shell.Application.Features.Directories.Commands.CreateDirectory;
using Shell.Application.Features.Directories.Commands.DeleteDirectory;
using Shell.Domain.Entities;
using Directory = Shell.Domain.Entities.Directory;
using File = System.IO.File;

namespace Shell.Application.Features.Directories.Commands.CopyDirectory
{
    public class CopyDirectoryCommandHandler(
        IShellDbContext dbContext,
        IFileSystemService fileSystemService,
        IMediator mediator)
        : IRequestHandler<CopyDirectoryCommand, string>
    {
        public async Task<string> Handle(CopyDirectoryCommand request, CancellationToken cancellationToken)
        {
            var existLocal = System.IO.Directory.Exists(request.Path);
            var existInDb = await dbContext.Directories
                .AnyAsync(directory => directory.Path.Equals(request.Path), cancellationToken);

            // Sync with local version
            if (!existLocal)
            {
                if (existInDb)
                    await mediator.Send(new DeleteDirectoryCommand { Path = request.Path }, cancellationToken);
                throw new NotFoundException(nameof(File), request.Path);
            }

            if (!existInDb)
                await mediator.Send(new CreateDirectoryCommand { Path = request.Path }, cancellationToken);

            // Check drive
            var directoryDrivePath = Path.GetPathRoot(request.NewPath)!;
            _ = await dbContext.Drives
                .SingleOrDefaultAsync(drive => drive.Name.Equals(directoryDrivePath), cancellationToken)
                ?? throw new NotFoundException(nameof(Drive), directoryDrivePath);

            // Path with directory name to copy properly
            var newPath = Path.GetFileName(request.NewPath).Equals(string.Empty)
                ? request.NewPath + Path.GetFileName(request.Path)
                : request.NewPath + Path.DirectorySeparatorChar + Path.GetFileName(request.Path);

            // Adds "copy" suffix if needed
            newPath = fileSystemService.GetUniqueNewPath(newPath);

            // Copy local
            try
            {
                fileSystemService.CopyDirectory(request.Path, newPath);
            }
            catch (Exception e)
            {
                throw new CopyException(nameof(Directory), new { request.Path, newPath }, e);
            }

            // Add to db
            if (!Path.GetFileName(newPath).Equals(string.Empty))
            {
                existInDb = await dbContext.Directories
                    .AnyAsync(directory => directory.Path.Equals(newPath), cancellationToken);

                if (!existInDb)
                    await mediator.Send(new CreateDirectoryCommand { Path = newPath }, cancellationToken);
            }

            return newPath;
        }
    }
}
