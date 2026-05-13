using TemplateProject.Application.Repositories;
using TemplateProject.Domain.Entity.App;
using TemplateProject.Persistance.Repositories;
using TemplateProject.Persistence.Context;

namespace TemplateProject.Persistence.Repositories;

internal sealed class AppUserRefreshTokenRepository : GenericRepository<AppUserRefreshToken>, IAppUserRefreshTokenRepository
{
    public AppUserRefreshTokenRepository(AppDbContext context) : base(context)
    {
    }
}
