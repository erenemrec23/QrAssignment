using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using TemplateProject.Domain.Entity;

namespace TemplateProject.Persistance.Configurations
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
