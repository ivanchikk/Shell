using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shell.Application.Common.Exceptions;
using Shell.Application.Common.Interfaces;
using Shell.Domain.Entities;

namespace Shell.Application.Features.Drives.Queries.GetDrive
{
    public class GetDriveQueryHandler(IShellDbContext dbContext, IMapper mapper)
        : IRequestHandler<GetDriveQuery, GetDriveQueryResponse>
    {
        public async Task<GetDriveQueryResponse> Handle(GetDriveQuery request, CancellationToken cancellationToken)
        {
            var drive = await dbContext.Drives
                .SingleOrDefaultAsync(drive => drive.Name.Equals(request.Name), cancellationToken)
                ?? throw new NotFoundException(nameof(Drive), request.Name);

            return mapper.Map<GetDriveQueryResponse>(drive);
        }
    }
}
