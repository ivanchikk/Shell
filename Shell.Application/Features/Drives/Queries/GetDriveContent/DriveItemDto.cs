namespace Shell.Application.Features.Drives.Queries.GetDriveContent
{
    public class DriveItemDto
    {
        public string Name { get; set; } = null!;
        public string Path { get; set; } = null!;
        public DateTime CreationDate { get; set; }
        public DateTime EditDate { get; set; }
        public bool IsDirectory { get; set; }
    }
}
