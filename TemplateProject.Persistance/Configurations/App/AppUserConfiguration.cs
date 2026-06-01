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

            builder.OwnsMany(x => x.AppUserRoles, roleBuilder =>
            {
                roleBuilder.ToTable("AppUserRoles");  
                roleBuilder.WithOwner().HasForeignKey("UserId");  
                roleBuilder.Property(r => r.Name).HasColumnName("Role"); 
                roleBuilder.HasKey("UserId", "Id"); 
            });

        }
    }
}
