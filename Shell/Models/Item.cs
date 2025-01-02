namespace Shell.Models
{
    public class Item
    {
        public string Name { get; set; } = null!;
        public string Path { get; set; } = null!;
        public DateTime CreationDate { get; set; }
        public DateTime EditDate { get; set; }
        public bool IsDirectory { get; set; }

        public override string ToString()
        {
            var type = IsDirectory ? "Dir" : "File";
            return $"{type,-10}{CreationDate,-20:g}{EditDate,-20:g}{Name}";
        }
    }
}
