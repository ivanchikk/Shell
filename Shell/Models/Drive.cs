namespace Shell.Models
{
    public class Drive
    {
        public string Name { get; set; } = null!;

        public override string ToString()
        {
            return $"Name: {Name}";
        }
    }
}
