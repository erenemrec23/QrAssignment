using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QrAssignment.Domain.Entity;

namespace QrAssignment.Persistance.Configurations
{
    public sealed class BrandConfiguration : IEntityTypeConfiguration<Brand>
    {
        public void Configure(EntityTypeBuilder<Brand> builder)
        {
            builder.ToTable("Brands"); 
             
            builder.Property(p => p.Name).HasMaxLength(150).IsRequired(); 
        }
    }
}
