using Microsoft.AspNetCore.Identity;


namespace TemplateProject.Domain.Entity.App
{
    public class AppRole : IdentityRole<Guid>
    {
        public virtual string Name { get; set; } = default!;

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

