using MediatR;
using Microsoft.EntityFrameworkCore;
using Shell.Application.Common.Exceptions;
using Shell.Application.Common.Interfaces;
using Shell.Domain.Entities;

namespace Shell.Application.Features.Drives.Commands.UpdateDrive
{
    public class UpdateDriveCommandHandler(IShellDbContext dbContext) : IRequestHandler<UpdateDriveCommand, Unit>
    {
        public async Task<Unit> Handle(UpdateDriveCommand request, CancellationToken cancellationToken)
        {
            var drive = await dbContext.Drives
                .SingleOrDefaultAsync(drive => drive.Name.Equals(request.Name), cancellationToken)
                ?? throw new NotFoundException(nameof(Drive), request.Name);

            drive.Name = request.NewName;

            await dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
