namespace Shell.Domain.Entities
{
    public class Directory
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Path { get; set; } = null!;
        public DateTime CreationDate { get; set; }
        public DateTime EditDate { get; set; }
        public int? ParentDirectoryId { get; set; }
        public int DriveId { get; set; }

        public Directory? ParentDirectory { get; set; }
        public Drive Drive { get; set; } = null!;
        public ICollection<Directory> ChildDirectories { get; set; } = new List<Directory>();
        public ICollection<File> Files { get; set; } = new List<File>();
    }
}
