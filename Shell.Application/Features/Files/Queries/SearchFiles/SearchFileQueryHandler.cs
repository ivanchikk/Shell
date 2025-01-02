using MediatR;
using Shell.Application.Features.Directories.Queries.GetDirectoryContent;

namespace Shell.Application.Features.Files.Queries.SearchFiles
{
    public class SearchFileQueryHandler(IMediator mediator)
        : IRequestHandler<SearchFileQuery, SearchFileQueryResponse>
    {
        public async Task<SearchFileQueryResponse> Handle(SearchFileQuery request, CancellationToken cancellationToken)
        {
            var content = await mediator.Send(new GetDirectoryContentQuery { Path = request.Path }, cancellationToken);

            var files = content.Content
                .Where(item => !item.IsDirectory)
                .Select(item => new FileItemDto
                {
                    Name = item.Name,
                    Path = item.Path,
                    CreationDate = item.CreationDate,
                    EditDate = item.EditDate
                });

            if (request.Name != null)
                files = files.Where(f => f.Name.Contains(request.Name));

            if (request.CreationDate != null)
                files = files.Where(f => f.CreationDate.Equals(request.CreationDate));

            return new SearchFileQueryResponse { Files = files.ToList() };
        }
    }
}
