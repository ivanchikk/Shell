using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using File = Shell.Domain.Entities.File;

namespace Shell.Persistence.EntityTypeConfigurations
{
    public class FileConfiguration : IEntityTypeConfiguration<File>
    {
        public void Configure(EntityTypeBuilder<File> builder)
        {
            builder.ToTable("Files");

            builder.HasKey(file => file.Id);
            builder.HasIndex(file => file.Id).IsUnique();

            builder.Property(file => file.Name)
                .HasMaxLength(256)
                .IsRequired();

            builder.Property(file => file.Path)
                .IsRequired();
            builder.HasIndex(file => file.Path)
                .IsUnique();

            builder.Property(file => file.CreationDate)
                .HasDefaultValue(DateTime.UtcNow)
                .IsRequired();

            builder.Property(file => file.EditDate)
                .HasDefaultValue(DateTime.UtcNow)
                .IsRequired();

            builder
                .HasOne(file => file.Directory)
                .WithMany(directory => directory.Files)
                .HasForeignKey(file => file.DirectoryId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(file => file.Drive)
                .WithMany(drive => drive.Files)
                .HasForeignKey(file => file.DriveId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
