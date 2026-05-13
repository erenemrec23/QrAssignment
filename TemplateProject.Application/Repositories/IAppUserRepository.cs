using QrAssignment.Domain.Entity.App;

namespace QrAssignment.Application.Repositories
{

    public interface IAppUserRepository  
    {
        Task<AppUser?> GetByIdWithRefreshTokenAsync(Guid id, CancellationToken cancellationToken = default);
        Task<AppUser?> GetByEmailWithRefreshTokenAsync (string email, CancellationToken cancellationToken = default);
    }
}
