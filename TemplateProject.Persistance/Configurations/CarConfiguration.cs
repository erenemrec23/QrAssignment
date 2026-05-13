using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System;
using System.Collections.Generic;
using System.Text;
using TemplateProject.Domain.Entity;

namespace TemplateProject.Persistance.Configurations
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
