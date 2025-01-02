namespace Shell.Domain.Entities
{
    public class File
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Path { get; set; } = null!;
        public DateTime CreationDate { get; set; }
        public DateTime EditDate { get; set; }
        public int? DirectoryId { get; set; }
        public int DriveId { get; set; }

        public Directory? Directory { get; set; }
        public Drive Drive { get; set; } = null!;
    }
}
