using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QrAssignment.Domain.Entity;
using QrAssignment.Persistance.Configurations.Base;

namespace QrAssignment.Persistance.Configurations
{
    public sealed class QrLocationConfiguration : BaseEntityConfiguration<QrLocation>
    {
        public override void Configure(EntityTypeBuilder<QrLocation> builder)
        {
            base.Configure(builder);

            builder.ToTable("QrLocations");

            builder.HasOne(c => c.ParentLocation)
                   .WithMany(b => b.SubLocations)
                   .HasForeignKey(c => c.ParentLocationId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(p => p.Name).HasMaxLength(250).IsRequired();
        }
    }
}