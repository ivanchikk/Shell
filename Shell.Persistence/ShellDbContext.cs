using Microsoft.EntityFrameworkCore;
using Shell.Application.Common.Interfaces;
using Shell.Domain.Entities;
using Shell.Persistence.EntityTypeConfigurations;
using Directory = Shell.Domain.Entities.Directory;
using File = Shell.Domain.Entities.File;

namespace Shell.Persistence
{
    public class ShellDbContext(DbContextOptions<ShellDbContext> options) : DbContext(options), IShellDbContext
    {
        public DbSet<File> Files { get; set; }
        public DbSet<Directory> Directories { get; set; }
        public DbSet<Drive> Drives { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new FileConfiguration());
            modelBuilder.ApplyConfiguration(new DirectoryConfiguration());
            modelBuilder.ApplyConfiguration(new DriveConfiguration());
        }
    }
}
