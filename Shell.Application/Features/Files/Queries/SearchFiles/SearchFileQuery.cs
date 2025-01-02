using MediatR;

namespace Shell.Application.Features.Files.Queries.SearchFiles
{
    public class SearchFileQuery : IRequest<SearchFileQueryResponse>
    {
        public string Path { get; set; } = null!;
        public string? Name { get; set; }
        public DateTime? CreationDate { get; set; }
    }
}
