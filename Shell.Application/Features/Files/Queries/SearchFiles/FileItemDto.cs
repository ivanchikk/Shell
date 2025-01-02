namespace Shell.Application.Features.Files.Queries.SearchFiles
{
    public class FileItemDto
    {
        public string Name { get; set; } = null!;
        public string Path { get; set; } = null!;
        public DateTime CreationDate { get; set; }
        public DateTime EditDate { get; set; }
    }
}
