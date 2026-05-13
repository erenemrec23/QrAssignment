using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TemplateProject.Domain.Abstractions;

namespace TemplateProject.Persistance.Configurations.Base
{
    public abstract class BaseEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : BaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(x => x.Id);
             
            builder.Property(x => x.RowVersion).IsRowVersion();
        }
    }
}
