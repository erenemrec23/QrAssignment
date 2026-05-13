using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QrAssignment.Domain.Entity;

namespace QrAssignment.Persistance.Configurations
{
    public sealed class CarConfiguration : IEntityTypeConfiguration<Car>
    {
        public void Configure(EntityTypeBuilder<Car> builder)
        {
            builder.ToTable("Cars"); 
              
            builder.HasOne(c => c.Brand)        
           .WithMany(b => b.Cars)         
           .HasForeignKey(c => c.BrandId)  
           .OnDelete(DeleteBehavior.Restrict); 

            builder.Property(p => p.Brand).HasMaxLength(50).IsRequired();
            builder.Property(p => p.Model).HasMaxLength(50).IsRequired();
        }
    }
}
