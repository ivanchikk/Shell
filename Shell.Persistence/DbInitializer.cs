using Shell.Domain.Entities;

namespace Shell.Persistence
{
    public class DbInitializer
    {
        public static void Initialize(ShellDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            AddDrives(context);
            //AddSomeData(context);
        }

        private static void AddDrives(ShellDbContext context)
        {
            var drives = DriveInfo.GetDrives();
            foreach (var drive in drives)
            {
                context.Add(new Drive { Name = drive.Name });
            }

            context.SaveChanges();
        }

        private static void AddSomeData(ShellDbContext context)
        {
            // Drives
            var drive1 = context.Drives.Single(drive => drive.Name.Equals("U:\\"));

            // Directories
            Domain.Entities.Directory directory1 = new()
            {
                Name = "dir1",
                Path = $"{drive1.Name}dir1",
                DriveId = drive1.Id,
            };
            context.Add(directory1);
            context.SaveChanges();

            Domain.Entities.Directory directory2 = new()
            {
                Name = "dir2",
                Path = $"{drive1.Name}dir1\\dir2",
                DriveId = drive1.Id,
                ParentDirectoryId = directory1.Id,
            };
            Domain.Entities.Directory directory3 = new()
            {
                Name = "dir3",
                Path = $"{drive1.Name}dir3",
                DriveId = drive1.Id,
            };
            context.AddRange(directory2, directory3);
            context.SaveChanges();

            // Files
            Domain.Entities.File file1 = new()
            {
                Name = "f1.txt",
                Path = $"{drive1.Name}f1.txt",
                DriveId = drive1.Id,
            };
            Domain.Entities.File file2 = new()
            {
                Name = "f2.txt",
                Path = $"{drive1.Name}{directory1.Name}\\f2.txt",
                DriveId = drive1.Id,
                DirectoryId = directory1.Id,
            };
            Domain.Entities.File file3 = new()
            {
                Name = "f3.txt",
                Path = $"{directory2.Path}\\f3.txt",
                DriveId = drive1.Id,
                DirectoryId = directory2.Id,
            };
            context.AddRange(file1, file2, file3);
            context.SaveChanges();
        }
    }
}
