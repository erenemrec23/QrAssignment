using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Entity.App;
using QrAssignment.Persistance.Context;

namespace QrAssignment.Persistance.Repositories;

internal sealed class AppUserRefreshTokenRepository : GenericRepository<AppUserRefreshToken>, IAppUserRefreshTokenRepository
{
    public AppUserRefreshTokenRepository(AppDbContext context) : base(context)
    {
    }
}
