//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;
//using QrAssignment.Domain.Entity.App;

//namespace QrAssignment.Persistance.Configurations.App
//{
//    public sealed class AppUserClaimConfiguration : IEntityTypeConfiguration<IdentityUserClaim<Guid>>
//    {
//        public void Configure(EntityTypeBuilder<IdentityUserClaim<Guid>> builder)
//        {
//            builder.ToTable("AppUserClaims"); 

//            builder.Property<byte[]>("RowVersion")
//                   .IsRowVersion();

//            builder.HasOne<AppUser>()
//                   .WithMany(u => u.Claims)
//                   .HasForeignKey(uc => uc.UserId)
//                   .IsRequired()
//                   .OnDelete(DeleteBehavior.Cascade);
//        }
//    }
//}