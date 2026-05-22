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

            builder.Property(x => x.Id)
                   .ValueGeneratedOnAdd(); 

            builder.HasOne(x => x.AppUser)
               .WithOne(u => u.RefreshToken)
               .HasForeignKey<AppUserRefreshToken>(x => x.AppUserId)
               .OnDelete(DeleteBehavior.Cascade); 
             
            builder.Property(x => x.RefreshToken)
                   .IsRequired()
                   .HasMaxLength(250); 

            builder.Property(x => x.RefreshTokenExpires)
                   .IsRequired();
        }
    }
}
