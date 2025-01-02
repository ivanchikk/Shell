using MediatR;
using Microsoft.EntityFrameworkCore;
using Shell.Application.Common.Exceptions;
using Shell.Application.Common.Interfaces;
using Shell.Domain.Entities;

namespace Shell.Application.Features.Drives.Commands.CreateDrive
{
    public class CreateDriveCommandHandler(IShellDbContext dbContext) : IRequestHandler<CreateDriveCommand, string>
    {
        public async Task<string> Handle(CreateDriveCommand request, CancellationToken cancellationToken)
        {
            var exist = await dbContext.Drives
                .AnyAsync(drive => drive.Name.Equals(request.Name), cancellationToken);

            if (exist)
                throw new DuplicateException(nameof(Drive), request.Name);

            var drive = new Drive
            {
                Name = request.Name,
            };

            await dbContext.Drives.AddAsync(drive, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return drive.Name;
        }
    }
}
