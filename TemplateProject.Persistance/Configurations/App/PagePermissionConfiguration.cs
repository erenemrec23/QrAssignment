using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QrAssignment.Domain.Entity.App;

namespace QrAssignment.Persistance.Configurations.App
{
    public sealed class PagePermissionConfiguration : IEntityTypeConfiguration<PagePermission>
    {
        public void Configure(EntityTypeBuilder<PagePermission> builder)
        {
            builder.ToTable("PagePermissions", t =>
                t.HasCheckConstraint(
                    "CK_PagePermission_SingleOwner",
                    // İkisinden tam biri dolu olmalı
                    "([UserId] IS NOT NULL AND [RoleId] IS NULL) OR ([UserId] IS NULL AND [RoleId] IS NOT NULL)"));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.PermissionValue).HasConversion<int>();

            // Sahip FK'leri — gerçek FK + cascade (sistemin geri kalanıyla tutarlı)
            builder.HasOne(x => x.User)
                   .WithMany()
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Role)
                   .WithMany()
                   .HasForeignKey(x => x.RoleId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Sayfa silinirse yetki uçmasın
            builder.HasOne(x => x.Page)
                   .WithMany()
                   .HasForeignKey(x => x.PageId)
                   .OnDelete(DeleteBehavior.Restrict);

            // "Aynı sahip + aynı sayfa" tek satır — iki taraf için filtreli unique index
            builder.HasIndex(x => new { x.UserId, x.PageId })
                   .IsUnique()
                   .HasFilter("[UserId] IS NOT NULL");

            builder.HasIndex(x => new { x.RoleId, x.PageId })
                   .IsUnique()
                   .HasFilter("[RoleId] IS NOT NULL");
        }
    }
}