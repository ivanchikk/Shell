namespace Shell.Models
{
    public class Directory
    {
        public string Name { get; set; } = null!;
        public string Path { get; set; } = null!;
        public DateTime CreationDate { get; set; }
        public DateTime EditDate { get; set; }

        public override string ToString()
        {
            return $"Name: {Name}, Path: {Path}, CreationDate: {CreationDate:g}, EditDate: {EditDate:g}";
        }
    }
}
