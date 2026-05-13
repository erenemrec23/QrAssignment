using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders; 
using TemplateProject.Domain.Entity.App;

namespace TemplateProject.Persistance.Configurations.App
{
    public sealed class AppUserRoleConfiguration : IEntityTypeConfiguration<AppUserRole>
    {
        public void Configure(EntityTypeBuilder<AppUserRole> builder)
        {
            builder.ToTable("AppUserRoles");

        }
    }

}
