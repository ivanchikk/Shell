using MediatR;

namespace Shell.Application.Features.Directories.Queries.GetDirectory
{
    public class GetDirectoryQuery : IRequest<GetDirectoryQueryResponse>
    {
        public string Path { get; set; } = null!;
    }
}
