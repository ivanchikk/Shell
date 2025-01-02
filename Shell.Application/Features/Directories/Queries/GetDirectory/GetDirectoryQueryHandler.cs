using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shell.Application.Common.Exceptions;
using Shell.Application.Common.Interfaces;
using Shell.Application.Features.Directories.Commands.CreateDirectory;
using Shell.Application.Features.Directories.Commands.DeleteDirectory;
using Directory = Shell.Domain.Entities.Directory;

namespace Shell.Application.Features.Directories.Queries.GetDirectory
{
    public class GetDirectoryQueryHandler(IShellDbContext dbContext, IMapper mapper, IMediator mediator)
        : IRequestHandler<GetDirectoryQuery, GetDirectoryQueryResponse>
    {
        public async Task<GetDirectoryQueryResponse> Handle(GetDirectoryQuery request, CancellationToken cancellationToken)
        {
            var directory = await dbContext.Directories
                .SingleOrDefaultAsync(directory => directory.Path.Equals(request.Path), cancellationToken);

            if (!System.IO.Directory.Exists(request.Path))
            {
                if (directory != null)
                    await mediator.Send(new DeleteDirectoryCommand { Path = request.Path }, cancellationToken);
                throw new NotFoundException(nameof(Directory), request.Path);
            }
            else if (directory == null)
            {
                await mediator.Send(new CreateDirectoryCommand { Path = request.Path }, cancellationToken);

                directory = await dbContext.Directories
                    .SingleOrDefaultAsync(directory => directory.Path.Equals(request.Path), cancellationToken);
            }

            return mapper.Map<GetDirectoryQueryResponse>(directory);
        }
    }
}
