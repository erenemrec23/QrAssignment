using TemplateProject.Application.Repositories;
using TemplateProject.Domain.Entity.App;
using TemplateProject.Persistance.Repositories;
using TemplateProject.Persistence.Context;

internal sealed class UserRepository : GenericRepository<AppUser>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context)
    {
    }
}