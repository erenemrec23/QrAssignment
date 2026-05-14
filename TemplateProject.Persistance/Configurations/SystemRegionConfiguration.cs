using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QrAssignment.Domain.Entity.System;

namespace QrAssignment.Persistance.Configurations
{
    public sealed class SystemRegionConfiguration : IEntityTypeConfiguration<SystemRegion>
    {
        public void Configure(EntityTypeBuilder<SystemRegion> builder)
        {
            builder.ToTable("SystemRegions");
            builder.HasOne(c => c.ParentRegion)
           .WithMany(b => b.SubLocations)
           .HasForeignKey(c => c.ParentRegionId)
           .OnDelete(DeleteBehavior.Restrict);

            builder.Property(p => p.Name).HasMaxLength(250).IsRequired();
            builder.Property(p => p.Code).HasMaxLength(250);
            builder.Property(p => p.Level).HasMaxLength(250).IsRequired();
        }
    }
}

