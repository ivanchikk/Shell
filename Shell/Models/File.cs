namespace Shell.Models
{
    public class File
    {
        public string Name { get; set; } = null!;
        public string Path { get; set; } = null!;
        public DateTime CreationDate { get; set; }
        public DateTime EditDate { get; set; }

        public override string ToString()
        {
            return $"{CreationDate,-20:g}{EditDate,-20:g}{Name,-20}{Path}";
        }
    }
}
