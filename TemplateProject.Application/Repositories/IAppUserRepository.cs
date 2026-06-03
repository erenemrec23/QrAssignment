using QrAssignment.Application.Features.AppUser.Queries.GetById;
using QrAssignment.Application.Features.AppUser.Queries.GetList;
using QrAssignment.Domain.Entity.App;

namespace QrAssignment.Application.Repositories
{

    public interface IAppUserRepository  
    {
        Task<AppUser?> GetByIdWithRefreshTokenAsync(Guid id, CancellationToken cancellationToken = default);
        Task<AppUser?> GetByEmailWithRefreshTokenAsync (string email, CancellationToken cancellationToken = default);

        Task<List<AppUserListItemDto>> GetList(CancellationToken cancellationToken);
        Task<AppUserItemDto> GetById(Guid? id, CancellationToken cancellationToken);
    }
}
