namespace Shell.Application.Features.Directories.Queries.GetDirectoryContent
{
    public class GetDirectoryContentQueryResponse
    {
        public IList<DirectoryItemDto> Content { get; set; } = null!;
    }
}
