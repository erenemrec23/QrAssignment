using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QrAssignment.Domain.Entity;

namespace QrAssignment.Persistance.Configurations
{
    public sealed class QrApplicantConfiguration : IEntityTypeConfiguration<QrApplicant>
    {
        public void Configure(EntityTypeBuilder<QrApplicant> builder)
        {
            builder.ToTable("QrApplicants");

            builder.Property(p => p.FirstName).HasMaxLength(250).IsRequired();
            builder.Property(p => p.LastName).HasMaxLength(250).IsRequired();
            builder.Property(p => p.Mail).HasMaxLength(250).IsRequired();
        }
    }
}
