using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QrAssignment.Domain.Entity.App;

namespace QrAssignment.Persistance.Configurations.App
{
    public sealed class AppUserRefreshTokenConfiguration : IEntityTypeConfiguration<AppUserRefreshToken>
    {
        public void Configure(EntityTypeBuilder<AppUserRefreshToken> builder)
        {
            builder.ToTable("AppUserRefreshTokens");

            builder.HasOne(x => x.AppUser)
               .WithOne(u => u.RefreshToken)
               .HasForeignKey<AppUserRefreshToken>(x => x.AppUserId)
               .OnDelete(DeleteBehavior.Cascade); // Ana kullanıcı silinirse, ona ait token tablosu da otomatik silinir.

            // 4. Property Kısıtlamaları (İyi bir veritabanı tasarımı için tavsiye edilir)
            builder.Property(x => x.RefreshToken)
                   .IsRequired()
                   .HasMaxLength(250); // Token uzunluğuna göre bir sınır koymak performansı artırır.

            builder.Property(x => x.RefreshTokenExpires)
                   .IsRequired();
        }
    }
}
