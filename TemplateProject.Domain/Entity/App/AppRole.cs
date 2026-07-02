using Microsoft.AspNetCore.Identity;
using QrAssignment.Domain.Abstractions;


namespace QrAssignment.Domain.Entity.App
{

    public class AppRole : IdentityRole<Guid>, IMustHaveTenant
    {
        public Guid? TenantId { get; set; }
        public virtual string Name { get; set; } = default!;

        public virtual DateTime CreatedDate { get; set; }
        public virtual DateTime? ModifiedDate { get; set; }

        public virtual bool? IsDeleted { get; set; }
        public static AppRole Create(string name)
        {
            return new AppRole()
            {
                Name = name
            };
        }
    }
}

