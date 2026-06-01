using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QrAssignment.Domain.Entity.App;

public sealed class AppUserRoleConfiguration : IEntityTypeConfiguration<AppUserRole>
{
    public void Configure(EntityTypeBuilder<AppUserRole> builder)
    {
        builder.ToTable("AppUserRoles");

        // 1. Primary Key Tanımı (Eğer BaseEntityConfiguration kullanmıyorsan)
        builder.HasKey(x => x.Id);

        // 2. Enum Dönüşümü (Kritik Nokta)
        // Veritabanında 1, 2, 3 gibi anlamsız sayılar yerine 'Admin', 'Manager' 
        // şeklinde string olarak yazması veritabanını okurken hayat kurtarır.
        builder.Property(x => x.AppRole)
               .HasConversion<string>()
               .IsRequired();

        // 3. İlişki (Relationship) ve Foreign Key Tanımı
        builder.HasOne(x => x.AppUser)
               .WithMany(x => x.AppUserRoles) // AppUser içindeki liste property'si
               .HasForeignKey(x => x.AppUserId)
               // 4. Silme Kuralı (Kritik)
               // Kullanıcı silinirse, o kullanıcıya ait tüm rol kayıtları da OTOMATİK silinsin.
               .OnDelete(DeleteBehavior.Cascade);
    }
}