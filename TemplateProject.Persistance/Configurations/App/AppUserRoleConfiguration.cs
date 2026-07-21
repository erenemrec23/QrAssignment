using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QrAssignment.Domain.Entity.App;

namespace QrAssignment.Persistance.Configurations.App
{
    public sealed class AppUserRoleConfiguration : IEntityTypeConfiguration<AppUserRole>
    {
        public void Configure(EntityTypeBuilder<AppUserRole> builder)
        {
            builder.ToTable("AppUserRoles");
             
            builder.HasKey(x => x.Id);
             
            builder.HasOne(x => x.AppUser)
                   .WithMany(x => x.AppUserRoles) 
                   .HasForeignKey(x => x.AppUserId) 
                   .OnDelete(DeleteBehavior.Cascade);  

            builder.HasOne(x => x.AppRole)
                   .WithMany()
                   .HasForeignKey(x => x.AppRoleId) 
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}