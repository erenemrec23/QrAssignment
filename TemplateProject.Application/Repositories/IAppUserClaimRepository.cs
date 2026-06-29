using QrAssignment.Application.Features.Permission.Queries.GetByUserId;
using QrAssignment.Domain.Entity.App;

namespace QrAssignment.Application.Repositories
{
    public interface IAppUserClaimRepository
    {
        Task<List<PermissionUserPageItemDto>> GetUserWithPermissionsAsync(Guid? userId, CancellationToken cancellationToken = default);
    }
}
