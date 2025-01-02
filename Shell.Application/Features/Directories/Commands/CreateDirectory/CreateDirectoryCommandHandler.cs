using MediatR;
using Microsoft.EntityFrameworkCore;
using Shell.Application.Common.Exceptions;
using Shell.Application.Common.Interfaces;
using Shell.Application.Features.Directories.Commands.DeleteDirectory;
using Shell.Application.Features.Files.Commands.CreateFile;
using Shell.Application.Features.Files.Commands.DeleteFile;
using Shell.Domain.Entities;
using Directory = Shell.Domain.Entities.Directory;

namespace Shell.Application.Features.Directories.Commands.CreateDirectory
{
    public class CreateDirectoryCommandHandler(
        IShellDbContext dbContext,
        IFileSystemService fileSystemService,
        IMediator mediator)
        : IRequestHandler<CreateDirectoryCommand, string>
    {
        public async Task<string> Handle(CreateDirectoryCommand request, CancellationToken cancellationToken)
        {
            var existInDb = await dbContext.Directories
                .AnyAsync(directory => directory.Path.Equals(request.Path), cancellationToken);
            var existLocal = System.IO.Directory.Exists(request.Path);

            if (existInDb)
            {
                if (existLocal)
                    throw new DuplicateException(nameof(Directory), request.Path);
                await mediator.Send(new DeleteDirectoryCommand { Path = request.Path }, cancellationToken);
            }

            var directoryDrivePath = Path.GetPathRoot(request.Path)!;
            var directoryDrive = await dbContext.Drives
                .SingleOrDefaultAsync(drive => drive.Name.Equals(directoryDrivePath), cancellationToken)
                ?? throw new NotFoundException(nameof(Drive), directoryDrivePath);

            // Create in db
            var path = Path.GetPathRoot(request.Path)!;
            var directoryNames = request.Path.Split(Path.DirectorySeparatorChar).Skip(1);
            Directory? parentDirectory = null;
            List<Directory> directories = [];

            foreach (var name in directoryNames)
            {
                path = Path.Combine(path, name);

                if (System.IO.File.Exists(path))
                    throw new Exception($"Can't create directory {Path.GetFileName(path)} because file with the same name exists");

                var directory = await dbContext.Directories
                    .SingleOrDefaultAsync(directory => directory.Path.Equals(path), cancellationToken);

                if (directory != null && !System.IO.Directory.Exists(path))
                    await mediator.Send(new DeleteDirectoryCommand { Path = path }, cancellationToken);

                if (directory == null)
                {
                    directory = new Directory
                    {
                        Name = name,
                        Path = path,
                        CreationDate = DateTime.UtcNow,
                        EditDate = DateTime.UtcNow,
                        ParentDirectory = parentDirectory,
                        ParentDirectoryId = parentDirectory?.Id,
                        Drive = directoryDrive,
                        DriveId = directoryDrive.Id,
                    };

                    await dbContext.Directories.AddAsync(directory, cancellationToken);
                }

                parentDirectory = directory;
                directories.Add(directory);
            }

            //await _dbContext.SaveChangesAsync(cancellationToken);

            //foreach (var dir in directories)
            //{
            //    var subdirectories = System.IO.Directory.Exists(dir.Path) ? System.IO.Directory.GetDirectories(dir.Path) : [];

            //    foreach (var subdirPath in subdirectories)
            //    {
            //        var subdirectory = await _dbContext.Directories
            //            .SingleOrDefaultAsync(directory => directory.Path.Equals(subdirPath), cancellationToken);

            //        if (subdirectory != null && !System.IO.Directory.Exists(subdirPath))
            //            await _mediator.Send(new DeleteDirectoryCommand { Path = subdirPath }, cancellationToken);

            //        if (subdirectory == null)
            //        {
            //            subdirectory = new Directory
            //            {
            //                Name = Path.GetFileName(subdirPath),
            //                Path = subdirPath,
            //                CreationDate = DateTime.UtcNow,
            //                EditDate = DateTime.UtcNow,
            //                ParentDirectory = dir,
            //                ParentDirectoryId = dir?.Id,
            //                Drive = directoryDrive,
            //                DriveId = directoryDrive.Id,
            //            };

            //            await _dbContext.Directories.AddAsync(subdirectory, cancellationToken);
            //        }
            //    }

            //    var filePathes = System.IO.Directory.Exists(dir.Path) ? System.IO.Directory.GetFiles(dir.Path) : [];

            //    foreach (var filePath in filePathes)
            //    {
            //        var file = await _dbContext.Files
            //            .SingleOrDefaultAsync(file => file.Path.Equals(filePath), cancellationToken);

            //        if (file != null && !System.IO.File.Exists(filePath))
            //            await _mediator.Send(new DeleteFileCommand { Path = filePath }, cancellationToken);

            //        if (file == null)
            //        {
            //            await _mediator.Send(new CreateFileCommand { Path = filePath }, cancellationToken);
            //        }
            //    }
            //}

            await dbContext.SaveChangesAsync(cancellationToken);

            // Create local
            if (!existLocal)
            {
                try
                {
                    fileSystemService.CreateDirectory(request.Path);
                }
                catch (Exception e)
                {
                    throw new CreateException(nameof(Directory), request.Path, e);
                }
            }

            return request.Path;
        }
    }
}
