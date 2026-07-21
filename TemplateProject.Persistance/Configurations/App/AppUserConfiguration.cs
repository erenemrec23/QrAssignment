using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QrAssignment.Domain.Entity.App;

namespace QrAssignment.Persistance.Configurations.App
{
    public sealed class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.ToTable("AppUser");
             
            builder.Property<byte[]>("RowVersion")
                   .IsRowVersion();
            //builder.OwnsMany(x => x.AppUserRoles, roleBuilder =>
            //{
            //    roleBuilder.ToTable("AppUserRoles");
            //    roleBuilder.WithOwner().HasForeignKey("UserId");
            //    roleBuilder.Property(r => r.Name).HasColumnName("Role");
            //    roleBuilder.HasKey("UserId", "Id");
            //});

            //builder.HasMany(u => u.Claims)
            //       .WithOne() // IdentityUserClaim sınıfının içinde AppUser'a dönen bir navigation property olmadığı için içi boş bırakılır
            //       .HasForeignKey(uc => uc.UserId)
            //       .IsRequired()
            //       .OnDelete(DeleteBehavior.Cascade)
            //       ;
        }
    }
}