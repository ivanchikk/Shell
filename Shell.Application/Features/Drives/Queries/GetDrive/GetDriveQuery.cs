using MediatR;

namespace Shell.Application.Features.Drives.Queries.GetDrive
{
    public class GetDriveQuery : IRequest<GetDriveQueryResponse>
    {
        public string Name { get; set; } = null!;
    }
}
