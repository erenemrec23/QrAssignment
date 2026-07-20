
using Microsoft.AspNetCore.Identity;
using QrAssignment.Domain.Abstractions;
using QrAssignment.Domain.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace QrAssignment.Domain.Entity.App
{
    public class AppUser : IdentityUser<Guid>, IBaseEntity, IMustHaveTenant, ISoftDelete
    {
        [Filterable]
        public virtual string FirstName { get; set; } = default!;
        [Filterable]
        public virtual string LastName { get; set; } = default!;
        [Filterable]
        public virtual string FullName => $"{FirstName} {LastName}";


        public Guid? TenantId { get; set; }
        public virtual DateTimeOffset CreatedDate { get; set; }
        public virtual DateTimeOffset? ModifiedDate { get; set; }
        public bool IsPassived { get; set; } = false;

        [Timestamp]
        public byte[] RowVersion { get; set; } = null!;


        [Filterable]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long RevNum { get; set; }

        public AppUserRefreshToken? RefreshToken { get; set; }
        public static AppUser Create(string firstName, string lastName, string userName, string email)
        {
            return new AppUser()
            {
                FirstName = firstName,
                LastName = lastName,
                UserName = userName,
                Email = email,

            };
        }
        public void Update(string firstName, string lastName, string userName, string email)
        {
            FirstName = firstName;
            LastName = lastName;
            UserName = userName;
            Email = email;


        }

        public List<AppUserRole> AppUserRoles { get; set; } = new List<AppUserRole>();

        public virtual ICollection<IdentityUserClaim<Guid>> Claims { get; set; }
        public Guid? CreatedByUserId { get; set; }
        public Guid? ModifiedByUserId { get; set; }
    }
}
