using MediatR;
using Microsoft.EntityFrameworkCore;
using Shell.Application.Common.Exceptions;
using Shell.Application.Common.Interfaces;
using Directory = Shell.Domain.Entities.Directory;

namespace Shell.Application.Features.Directories.Commands.DeleteDirectory
{
    public class DeleteDirectoryCommandHandler(IShellDbContext dbContext, IFileSystemService fileSystemService)
        : IRequestHandler<DeleteDirectoryCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteDirectoryCommand request, CancellationToken cancellationToken)
        {
            var directory = await dbContext.Directories
                .SingleOrDefaultAsync(directory => directory.Path.Equals(request.Path), cancellationToken);
            var existLocal = System.IO.Directory.Exists(request.Path);

            if (directory == null && !existLocal)
                throw new NotFoundException(nameof(Directory), request.Path);

            if (existLocal)
            {
                try
                {
                    fileSystemService.DeleteDirectory(request.Path);
                }
                catch (Exception e)
                {
                    throw new DeleteException(nameof(Directory), request.Path, e);
                }
            }

            if (directory != null)
            {
                dbContext.Directories.Remove(directory);
                await dbContext.SaveChangesAsync(cancellationToken);
            }

            return Unit.Value;
        }
    }
}
