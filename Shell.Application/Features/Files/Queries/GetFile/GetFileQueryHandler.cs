using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shell.Application.Common.Exceptions;
using Shell.Application.Common.Interfaces;
using Shell.Application.Features.Files.Commands.CreateFile;
using Shell.Application.Features.Files.Commands.DeleteFile;

namespace Shell.Application.Features.Files.Queries.GetFile
{
    public class GetFileQueryHandler(IShellDbContext dbContext, IMapper mapper, IMediator mediator)
        : IRequestHandler<GetFileQuery, GetFileQueryResponse>
    {
        public async Task<GetFileQueryResponse> Handle(GetFileQuery request, CancellationToken cancellationToken)
        {
            var file = await dbContext.Files
                .SingleOrDefaultAsync(file => file.Path.Equals(request.Path), cancellationToken);

            if (!System.IO.File.Exists(request.Path))
            {
                if (file != null)
                    await mediator.Send(new DeleteFileCommand { Path = request.Path }, cancellationToken);
                throw new NotFoundException(nameof(File), request.Path);
            }
            else if (file == null)
            {
                await mediator.Send(new CreateFileCommand { Path = request.Path }, cancellationToken);

                file = await dbContext.Files
                    .SingleOrDefaultAsync(file => file.Path.Equals(request.Path), cancellationToken);
            }

            return mapper.Map<GetFileQueryResponse>(file);
        }
    }
}
