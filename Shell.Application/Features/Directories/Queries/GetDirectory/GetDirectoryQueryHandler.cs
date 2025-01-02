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
    public class GetDirectoryQueryHandler : IRequestHandler<GetDirectoryQuery, GetDirectoryQueryResponse>
    {
        private readonly IShellDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public GetDirectoryQueryHandler(IShellDbContext dbContext, IMapper mapper, IMediator mediator) =>
            (_dbContext, _mapper, _mediator) = (dbContext, mapper, mediator);

        public async Task<GetDirectoryQueryResponse> Handle(GetDirectoryQuery request, CancellationToken cancellationToken)
        {
            var directory = await _dbContext.Directories
                .SingleOrDefaultAsync(directory => directory.Path.Equals(request.Path), cancellationToken);

            if (!System.IO.Directory.Exists(request.Path))
            {
                if (directory != null)
                    await _mediator.Send(new DeleteDirectoryCommand { Path = request.Path }, cancellationToken);
                throw new NotFoundException(nameof(Directory), request.Path);
            }
            else if (directory == null)
            {
                await _mediator.Send(new CreateDirectoryCommand { Path = request.Path }, cancellationToken);

                directory = await _dbContext.Directories
                    .SingleOrDefaultAsync(directory => directory.Path.Equals(request.Path), cancellationToken);
            }

            return _mapper.Map<GetDirectoryQueryResponse>(directory);
        }
    }
}
