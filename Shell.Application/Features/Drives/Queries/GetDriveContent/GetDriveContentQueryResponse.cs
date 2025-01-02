namespace Shell.Application.Features.Drives.Queries.GetDriveContent
{
    public class GetDriveContentQueryResponse
    {
        public IList<DriveItemDto> Content { get; set; } = null!;
    }
}
