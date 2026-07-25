using QrAssignment.Application.DTOs.List;
using QrAssignment.Application.Features.AppRole.Queries.GetList;
using QrAssignment.Application.Features.Tenants.DTOs;
using QrAssignment.Application.Interfaces;
using QrAssignment.Domain.Entity.App;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Repositories
{
    public interface IAppRoleRepository 
    {
        Task<Paginate<RoleListItemDto>> GetDtoListAsync(PageRequestBaseDto request, CancellationToken ct = default);
        Task<Paginate<RoleListItemDto>> GetPassivedDtoListAsync(PageRequestBaseDto request, CancellationToken ct = default);
        Task<List<RoleListItemDto>> GetExportListAsync(PageRequestBaseDto request, CancellationToken ct = default);
        Task<RoleListItemDto?> GetDtoByIdAsync(Guid id, CancellationToken ct = default);
        Task<RoleListItemDto?> GetPassivedDtoByIdAsync(Guid id, CancellationToken ct = default);
        Task BulkDelete(List<Guid> ids, CancellationToken ct);
    }
}
 