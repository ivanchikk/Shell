using MediatR;

namespace Shell.Application.Features.Directories.Queries.GetDirectoryContent
{
    public class GetDirectoryContentQuery : IRequest<GetDirectoryContentQueryResponse>
    {
        public string Path { get; set; } = null!;
    }
}
