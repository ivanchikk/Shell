using MediatR;
using Microsoft.EntityFrameworkCore;
using Shell.Application.Common.Exceptions;
using Shell.Application.Common.Interfaces;
using File = Shell.Domain.Entities.File;

namespace Shell.Application.Features.Files.Commands.DeleteFile
{
    public class DeleteFileCommandHandler(IShellDbContext dbContext, IFileSystemService fileSystemService)
        : IRequestHandler<DeleteFileCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteFileCommand request, CancellationToken cancellationToken)
        {
            var file = await dbContext.Files
                .SingleOrDefaultAsync(file => file.Path.Equals(request.Path), cancellationToken);
            var existLocal = System.IO.File.Exists(request.Path);

            if (file == null && !existLocal)
                throw new NotFoundException(nameof(File), request.Path);

            if (existLocal)
            {
                try
                {
                    fileSystemService.DeleteFile(request.Path);
                }
                catch (Exception e)
                {
                    throw new DeleteException(nameof(File), request.Path, e);
                }
            }

            if (file != null)
            {
                dbContext.Files.Remove(file);
                await dbContext.SaveChangesAsync(cancellationToken);
            }

            return Unit.Value;
        }
    }
}
