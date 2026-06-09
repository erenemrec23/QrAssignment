
using Microsoft.AspNetCore.Identity;
using QrAssignment.Domain.Abstractions;
using System.ComponentModel.DataAnnotations;


namespace QrAssignment.Domain.Entity.App
{
    public class AppUser :  IdentityUser<Guid>, IBaseEntity, IMustHaveTenant
    {
        public Guid? TenantId { get; set; }
        public virtual string FirstName { get; set; } = default!;
        public virtual string LastName { get; set; } = default!;
        public virtual string FullName => $"{FirstName} {LastName}";

         
        public virtual DateTimeOffset CreatedDate { get; set; }
        public virtual DateTimeOffset? ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
         
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
    }
}
