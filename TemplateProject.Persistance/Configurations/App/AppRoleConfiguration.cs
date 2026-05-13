using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders; 
using TemplateProject.Domain.Entity.App;

namespace TemplateProject.Persistance.Configurations.App
{
    public sealed class AppRoleConfiguration : IEntityTypeConfiguration<AppRole>
    {
        public void Configure(EntityTypeBuilder<AppRole> builder)
        {
            builder.ToTable("AppRoles");
        }
    }
}
