using QrAssignment.Application.Features.Users.Queries.DTOs;
using QrAssignment.Domain.Entity.App;

namespace QrAssignment.Application.Repositories
{

    public interface IAppUserRepository  
    {
        Task<AppUser?> GetByIdWithRefreshTokenAsync(Guid id, CancellationToken cancellationToken = default);
        Task<AppUser?> GetByEmailWithRefreshTokenAsync (string email, CancellationToken cancellationToken = default);

        Task<List<AppUserListItemDto>> GetList(CancellationToken cancellationToken);
        Task<AppUserItemDto> GetById(Guid? id, CancellationToken cancellationToken);
        Task<List<AppUserLookUpListItemDto>> GetLookUpList(CancellationToken cancellationToken);
    }
    }
