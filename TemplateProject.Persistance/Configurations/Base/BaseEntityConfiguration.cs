using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QrAssignment.Domain.Abstractions;

namespace QrAssignment.Persistance.Configurations.Base
{
    public abstract class BaseEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : BaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.RowVersion).IsRowVersion();

            builder.Property(x => x.RevNum)
               .ValueGeneratedOnAdd()
               .UseIdentityColumn();

            builder.Property(x => x.IsPassived).HasColumnName("IsPassived").HasDefaultValue(false);

            // --- İLİŞKİ YAPILANDIRMASI ---
            builder.HasOne(x => x.CreatedByUser)
                   .WithMany() // User tarafından geriye koleksiyon tutmaya gerek yok
                   .HasForeignKey(x => x.CreatedByUserId)
                   .OnDelete(DeleteBehavior.Restrict); // Cascade delete çakışmasını önler

            builder.HasOne(x => x.ModifiedByUser)
                   .WithMany()
                   .HasForeignKey(x => x.ModifiedByUserId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}