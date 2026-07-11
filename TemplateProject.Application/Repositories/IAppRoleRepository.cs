using QrAssignment.Application.DTOs.List;
using QrAssignment.Application.Features.AppRole.Queries.GetList;
using QrAssignment.Application.Interfaces;
using QrAssignment.Domain.Entity.App;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Repositories
{
    public interface IAppRoleRepository 
    {
        Task<Paginate<RoleListItemDto>> GetListAsync(PageRequestBaseDto request, CancellationToken cancellationToken = default);
        Task<RoleListItemDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
 