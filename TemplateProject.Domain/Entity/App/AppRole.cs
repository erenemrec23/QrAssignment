using Microsoft.AspNetCore.Identity;
using QrAssignment.Domain.Abstractions;
using QrAssignment.Domain.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace QrAssignment.Domain.Entity.App
{

    public class AppRole : IdentityRole<Guid>, IMustHaveTenant, IBaseEntity
    {
        public Guid? TenantId { get; set; }
        [Filterable]
        public override string Name { get; set; } = default!;

        public virtual DateTimeOffset CreatedDate { get; set; }
        public virtual DateTimeOffset? ModifiedDate { get; set; }

        public virtual bool? IsDeleted { get; set; }
        public Guid? CreatedByUserId { get; set; }
        public Guid? ModifiedByUserId { get; set; }
        [Filterable]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long RevNum { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; } = null!;
        bool IBaseEntity.IsDeleted { get; set; }

        public static AppRole Create(string name)
        {
            return new AppRole()
            {
                Name = name
            };
        }
    }
}

