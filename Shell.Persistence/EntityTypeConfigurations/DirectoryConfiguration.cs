using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Directory = Shell.Domain.Entities.Directory;

namespace Shell.Persistence.EntityTypeConfigurations
{
    public class DirectoryConfiguration : IEntityTypeConfiguration<Directory>
    {
        public void Configure(EntityTypeBuilder<Directory> builder)
        {
            builder.ToTable("Directories");

            builder.HasKey(directory => directory.Id);
            builder.HasIndex(directory => directory.Id).IsUnique();

            builder.Property(directory => directory.Name)
                .HasMaxLength(256)
                .IsRequired();

            builder.Property(directory => directory.Path)
                .IsRequired();
            builder.HasIndex(directory => directory.Path)
                .IsUnique();

            builder.Property(directory => directory.CreationDate)
                .HasDefaultValue(DateTime.UtcNow)
                .IsRequired();

            builder.Property(directory => directory.EditDate)
                .HasDefaultValue(DateTime.UtcNow)
                .IsRequired();

            //builder
            //    .HasMany(directory => directory.Files)
            //    .WithOne(file => file.Directory)
            //    .HasForeignKey(file => file.DirectoryId);

            builder
                .HasOne(directory => directory.ParentDirectory)
                .WithMany(directory => directory.ChildDirectories)
                .HasForeignKey(directory => directory.ParentDirectoryId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(directory => directory.Drive)
                .WithMany(drive => drive.Directories)
                .HasForeignKey(directory => directory.DriveId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
