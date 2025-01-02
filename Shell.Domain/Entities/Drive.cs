namespace Shell.Domain.Entities
{
    public class Drive
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public ICollection<Directory> Directories { get; set; } = new List<Directory>();
        public ICollection<File> Files { get; set; } = new List<File>();
    }
}
