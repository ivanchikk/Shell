namespace Shell.Application.Features.Directories.Queries.GetDirectoryContent
{
    public class DirectoryItemDto
    {
        public string Name { get; set; } = null!;
        public string Path { get; set; } = null!;
        public DateTime CreationDate { get; set; }
        public DateTime EditDate { get; set; }
        public bool IsDirectory { get; set; }
    }
}
