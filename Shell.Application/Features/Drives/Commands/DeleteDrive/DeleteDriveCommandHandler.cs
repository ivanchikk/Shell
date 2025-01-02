using MediatR;
using Microsoft.EntityFrameworkCore;
using Shell.Application.Common.Exceptions;
using Shell.Application.Common.Interfaces;
using Shell.Domain.Entities;

namespace Shell.Application.Features.Drives.Commands.DeleteDrive
{
    public class DeleteDriveCommandHandler(IShellDbContext dbContext) : IRequestHandler<DeleteDriveCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteDriveCommand request, CancellationToken cancellationToken)
        {
            var drive = await dbContext.Drives
                .SingleOrDefaultAsync(drive => drive.Name.Equals(request.Name), cancellationToken)
                ?? throw new NotFoundException(nameof(Drive), request.Name);

            dbContext.Drives.Remove(drive);
            await dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
