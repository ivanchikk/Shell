using MediatR;

namespace Shell.Application.Features.Files.Queries.GetFile
{
    public class GetFileQuery : IRequest<GetFileQueryResponse>
    {
        public string Path { get; set; } = null!;
    }
}
