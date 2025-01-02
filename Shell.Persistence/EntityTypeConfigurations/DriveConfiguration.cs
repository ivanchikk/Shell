using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shell.Domain.Entities;

namespace Shell.Persistence.EntityTypeConfigurations
{
    public class DriveConfiguration : IEntityTypeConfiguration<Drive>
    {
        public void Configure(EntityTypeBuilder<Drive> builder)
        {
            builder.ToTable("Drives");

            builder.HasKey(drive => drive.Id);
            builder.HasIndex(drive => drive.Id).IsUnique();

            builder.Property(drive => drive.Name)
                .HasMaxLength(256)
                .IsRequired();
            builder.HasIndex(drive => drive.Name).IsUnique();

            //builder
            //    .HasMany(drive => drive.Files)
            //    .WithOne(file => file.Drive)
            //    .HasForeignKey(file => file.DriveId);

            //builder
            //    .HasMany(drive => drive.Directories)
            //    .WithOne(directory => directory.Drive)
            //    .HasForeignKey(directory => directory.DriveId);
        }
    }
}
