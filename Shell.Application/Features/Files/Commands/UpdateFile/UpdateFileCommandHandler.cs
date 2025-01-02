using MediatR;
using Microsoft.EntityFrameworkCore;
using Shell.Application.Common.Exceptions;
using Shell.Application.Common.Interfaces;
using Shell.Application.Features.Directories.Commands.CreateDirectory;
using Shell.Application.Features.Files.Commands.CreateFile;
using Shell.Application.Features.Files.Commands.DeleteFile;
using Shell.Domain.Entities;
using File = Shell.Domain.Entities.File;

namespace Shell.Application.Features.Files.Commands.UpdateFile
{
    public class UpdateFileCommandHandler(
        IShellDbContext dbContext,
        IFileSystemService fileSystemService,
        IMediator mediator)
        : IRequestHandler<UpdateFileCommand, Unit>
    {
        public async Task<Unit> Handle(UpdateFileCommand request, CancellationToken cancellationToken)
        {
            var existLocal = System.IO.File.Exists(request.Path);
            var file = await dbContext.Files
                .SingleOrDefaultAsync(f => f.Path.Equals(request.Path), cancellationToken);

            // Sync with local version
            if (!existLocal)
            {
                if (file != null)
                    await mediator.Send(new DeleteFileCommand { Path = request.Path }, cancellationToken);
                throw new NotFoundException(nameof(File), request.Path);
            }
            else if (file == null)
            {
                await mediator.Send(new CreateFileCommand { Path = request.Path }, cancellationToken);

                file = await dbContext.Files
                    .SingleOrDefaultAsync(f => f.Path.Equals(request.Path), cancellationToken);
            }

            // Sync destination with local version
            var existLocalDestination = System.IO.File.Exists(request.NewPath);
            var existInDbDestination = await dbContext.Files
                .AnyAsync(f => f.Path.Equals(request.NewPath), cancellationToken);

            if (!existLocalDestination)
            {
                if (existInDbDestination)
                    await mediator.Send(new DeleteFileCommand { Path = request.NewPath }, cancellationToken);
            }
            else if (!existInDbDestination)
                await mediator.Send(new CreateFileCommand { Path = request.NewPath }, cancellationToken);

            if (Path.Exists(request.NewPath))
                throw new Exception($"Can't rename file to '{Path.GetFileName(request.NewPath)}' because file or directory with the same name exist");

            var fileName = Path.GetFileName(request.Path);
            var newFileName = Path.GetFileName(request.NewPath);
            var fileDriveName = Path.GetPathRoot(request.Path)!;
            var newFileDriveName = Path.GetPathRoot(request.NewPath)!;
            var fileDirectoryPath = Path.GetDirectoryName(request.Path)!;
            var newFileDirectoryPath = Path.GetDirectoryName(request.NewPath)!;

            if (!fileName.Equals(newFileName))
            {
                file.Name = newFileName;
            }

            if (!fileDriveName.Equals(newFileDriveName))
            {
                var drive = await dbContext.Drives
                    .SingleOrDefaultAsync(drive => drive.Name.Equals(newFileDriveName), cancellationToken)
                    ?? throw new NotFoundException(nameof(Drive), newFileDriveName);

                file.DriveId = drive.Id;
                file.Drive = drive;
            }

            if (!fileDirectoryPath.Equals(newFileDirectoryPath))
            {
                if (!newFileDirectoryPath.Equals(newFileDriveName))
                {
                    // Create destination directory
                    var existInDb = await dbContext.Directories
                        .AnyAsync(directory => directory.Path.Equals(newFileDirectoryPath), cancellationToken);

                    if (!existInDb)
                        await mediator.Send(new CreateDirectoryCommand { Path = newFileDirectoryPath }, cancellationToken);

                    var directory = await dbContext.Directories
                        .SingleAsync(directory => directory.Path.Equals(newFileDirectoryPath), cancellationToken);

                    file.DirectoryId = directory.Id;
                    file.Directory = directory;
                }
                else
                {
                    file.DirectoryId = null;
                    file.Directory = null;
                }
            }

            file.EditDate = DateTime.UtcNow;
            file.Path = request.NewPath;

            // Update local
            if (existLocal)
            {
                try
                {
                    fileSystemService.UpdateFile(request.Path, request.NewPath);
                }
                catch (Exception e)
                {
                    throw new UpdateException(nameof(File), request.Path, e);
                }
            }

            // Update in db
            await dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
