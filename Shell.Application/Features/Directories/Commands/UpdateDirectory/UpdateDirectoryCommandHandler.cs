using MediatR;
using Microsoft.EntityFrameworkCore;
using Shell.Application.Common.Exceptions;
using Shell.Application.Common.Interfaces;
using Shell.Application.Features.Directories.Commands.CreateDirectory;
using Shell.Application.Features.Directories.Commands.DeleteDirectory;
using Shell.Domain.Entities;
using Directory = System.IO.Directory;

namespace Shell.Application.Features.Directories.Commands.UpdateDirectory
{
    public class UpdateDirectoryCommandHandler(
        IShellDbContext dbContext,
        IFileSystemService fileSystemService,
        IMediator mediator)
        : IRequestHandler<UpdateDirectoryCommand, Unit>
    {
        public async Task<Unit> Handle(UpdateDirectoryCommand request, CancellationToken cancellationToken)
        {
            var existLocal = System.IO.Directory.Exists(request.Path);
            var directory = await dbContext.Directories
                .SingleOrDefaultAsync(directory => directory.Path.Equals(request.Path), cancellationToken);

            // Sync with local version
            if (!existLocal)
            {
                if (directory != null)
                    await mediator.Send(new DeleteDirectoryCommand { Path = request.Path }, cancellationToken);
                throw new NotFoundException(nameof(Directory), request.Path);
            }
            else if (directory == null)
            {
                await mediator.Send(new CreateDirectoryCommand { Path = request.Path }, cancellationToken);

                directory = await dbContext.Directories
                    .SingleOrDefaultAsync(d => d.Path.Equals(request.Path), cancellationToken);
            }

            // Sync destination with local version
            var existLocalDestination = System.IO.Directory.Exists(request.NewPath);
            var existInDbDestination = await dbContext.Directories
                .AnyAsync(d => d.Path.Equals(request.NewPath), cancellationToken);

            if (!existLocalDestination)
            {
                if (existInDbDestination)
                    await mediator.Send(new DeleteDirectoryCommand { Path = request.NewPath }, cancellationToken);
            }
            else if (!existInDbDestination)
                await mediator.Send(new CreateDirectoryCommand { Path = request.NewPath }, cancellationToken);

            if (Path.Exists(request.NewPath))
                throw new Exception($"Can't rename directory to '{Path.GetFileName(request.NewPath)}' because file or directory with the same name exist");

            var directoryName = Path.GetFileName(request.Path);
            var newDirectoryName = Path.GetFileName(request.NewPath);
            var directoryDriveName = Path.GetPathRoot(request.Path)!;
            var newDirectoryDriveName = Path.GetPathRoot(request.NewPath)!;
            var parentDirectoryPath = Path.GetDirectoryName(request.Path)!;
            var newParentDirectoryPath = Path.GetDirectoryName(request.NewPath)!;

            if (!directoryName.Equals(newDirectoryName))
            {
                directory.Name = newDirectoryName;
            }

            if (!directoryDriveName.Equals(newDirectoryDriveName))
            {
                var drive = await dbContext.Drives
                    .SingleOrDefaultAsync(drive => drive.Name.Equals(newDirectoryDriveName), cancellationToken)
                    ?? throw new NotFoundException(nameof(Drive), newDirectoryDriveName);

                directory.DriveId = drive.Id;
                directory.Drive = drive;
            }

            if (!parentDirectoryPath.Equals(newParentDirectoryPath))
            {
                if (!newParentDirectoryPath.Equals(newDirectoryDriveName))
                {
                    // Create destination directory
                    var existInDb = await dbContext.Directories
                        .AnyAsync(d => d.Path.Equals(newParentDirectoryPath), cancellationToken);

                    if (!existInDb)
                        await mediator.Send(new CreateDirectoryCommand { Path = newParentDirectoryPath }, cancellationToken);

                    var parentDirectory = await dbContext.Directories
                        .SingleAsync(d => d.Path.Equals(newParentDirectoryPath), cancellationToken);

                    directory.ParentDirectoryId = parentDirectory.Id;
                    directory.ParentDirectory = parentDirectory;

                }
                else
                {
                    directory.ParentDirectoryId = null;
                    directory.ParentDirectory = null;
                }
            }

            directory.EditDate = DateTime.UtcNow;
            directory.Path = request.NewPath;

            // Update local
            if (existLocal)
            {
                try
                {
                    fileSystemService.UpdateDirectory(request.Path, request.NewPath);
                }
                catch (Exception e)
                {
                    throw new UpdateException(nameof(Directory), request.Path, e);
                }
            }

            // Update in db
            await dbContext.SaveChangesAsync(cancellationToken);

            // Update old in db
            var directoriesToUpdate = await dbContext.Directories
                .Where(d => d.ParentDirectory != null && d.ParentDirectory.Path.StartsWith(request.Path))
                .ToListAsync(cancellationToken);

            foreach (var directoryToUpdate in directoriesToUpdate)
            {
                directoryToUpdate.Path = directoryToUpdate.Path.Replace(request.Path, request.NewPath);
            }

            var filesToUpdate = await dbContext.Files
                .Where(file => file.Directory != null && file.Directory.Path.StartsWith(request.Path))
                .ToListAsync(cancellationToken);

            foreach (var fileToUpdate in filesToUpdate)
            {
                fileToUpdate.Path = fileToUpdate.Path.Replace(request.Path, request.NewPath);
            }

            await dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
