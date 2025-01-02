using Microsoft.EntityFrameworkCore;
using Shell.Domain.Entities;
using Directory = Shell.Domain.Entities.Directory;
using File = Shell.Domain.Entities.File;

namespace Shell.Application.Common.Interfaces
{
    public interface IShellDbContext
    {
        DbSet<File> Files { get; set; }
        DbSet<Directory> Directories { get; set; }
        DbSet<Drive> Drives { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
