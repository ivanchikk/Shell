using MediatR;

namespace Shell.Application.Features.Drives.Queries.GetDriveContent
{
    public class GetDriveContentQuery : IRequest<GetDriveContentQueryResponse>
    {
        public string Name { get; set; } = null!;
    }
}
