
using Microsoft.AspNetCore.Identity;
using QrAssignment.Domain.Abstractions;
using QrAssignment.Domain.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace QrAssignment.Domain.Entity.App
{
    public class AppUser : IdentityUser<Guid>, IBaseEntity, IMustHaveTenant, ISoftDelete
    {
        private string _firstName = default!;
        private string _lastName = default!;
        [Filterable]
        public virtual string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                UpdateFullName();
            }
        }

        [Filterable]
        public virtual string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                UpdateFullName();
            }
        }

        // Artık hem veritabanında gerçek bir kolon hem de filtrelenebilir!
        [Filterable]
        public virtual string FullName { get; private set; } = default!;

        private void UpdateFullName()
        {
            FullName = $"{_firstName} {_lastName}".Trim();
        }


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
