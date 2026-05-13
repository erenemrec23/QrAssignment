using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;


namespace TemplateProject.Domain.Entity.App
{
    public class AppUserRole : IdentityRole<Guid>
    {

        [ForeignKey("AppUser")]
        public Guid? AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        [ForeignKey("AppRole")]
        public Guid? AppRoleId { get; set; }
        public AppRole AppRole { get; set; }

        public virtual DateTime CreatedDate { get; set; }
        public virtual DateTime? ModifiedDate { get; set; }

        public virtual bool? IsDeleted { get; set; }
        public static AppUserRole Create(string name)
        {
            return new AppUserRole()
            {
                Name = name
            };
        }
    }

}
