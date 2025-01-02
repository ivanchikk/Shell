using MediatR;
using Microsoft.EntityFrameworkCore;
using Shell.Application.Common.Exceptions;
using Shell.Application.Common.Interfaces;
using Shell.Application.Features.Directories.Commands.CreateDirectory;
using Shell.Application.Features.Files.Commands.CreateFile;
using Shell.Application.Features.Files.Commands.DeleteFile;
using Shell.Domain.Entities;
using File = Shell.Domain.Entities.File;

namespace Shell.Application.Features.Files.Commands.CopyFile
{
    public class CopyFileCommandHandler(
        IShellDbContext dbContext,
        IFileSystemService fileSystemService,
        IMediator mediator)
        : IRequestHandler<CopyFileCommand, string>
    {
        public async Task<string> Handle(CopyFileCommand request, CancellationToken cancellationToken)
        {
            var existLocal = System.IO.File.Exists(request.Path);
            var existInDb = await dbContext.Files
                .AnyAsync(file => file.Path.Equals(request.Path), cancellationToken);

            // Sync with local version
            if (!existLocal)
            {
                if (existInDb)
                    await mediator.Send(new DeleteFileCommand { Path = request.Path }, cancellationToken);
                throw new NotFoundException(nameof(File), request.Path);
            }

            if (!existInDb)
                await mediator.Send(new CreateFileCommand { Path = request.Path }, cancellationToken);

            // Check drive
            var fileDrivePath = Path.GetPathRoot(request.NewPath)!;
            _ = await dbContext.Drives
                .SingleOrDefaultAsync(drive => drive.Name.Equals(fileDrivePath), cancellationToken)
                ?? throw new NotFoundException(nameof(Drive), fileDrivePath);

            // Create destination directory if needed
            if (!Path.GetFileName(request.NewPath).Equals(string.Empty))
            {
                existInDb = await dbContext.Directories
                    .AnyAsync(directory => directory.Path.Equals(request.NewPath), cancellationToken);

                if (!existInDb)
                    await mediator.Send(new CreateDirectoryCommand { Path = request.NewPath }, cancellationToken);
            }

            // Path with file name to copy properly
            var newPath = Path.GetFileName(request.NewPath).Equals(string.Empty)
                ? request.NewPath + Path.GetFileName(request.Path)
                : request.NewPath + Path.DirectorySeparatorChar + Path.GetFileName(request.Path);

            // Adds "copy" suffix if needed
            newPath = fileSystemService.GetUniqueNewPath(newPath);

            // Copy local
            try
            {
                fileSystemService.CopyFile(request.Path, newPath);
            }
            catch (Exception e)
            {
                throw new CopyException(nameof(File), new { request.Path, newPath }, e);
            }

            // Add to db
            existInDb = await dbContext.Files
                .AnyAsync(file => file.Path.Equals(newPath), cancellationToken);

            if (!existInDb)
                await mediator.Send(new CreateFileCommand { Path = newPath }, cancellationToken);

            return newPath;
        }
    }
}
