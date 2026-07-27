using QrAssignment.Application.DTOs.List;
using QrAssignment.Application.Features.Roles.DTOs;
using QrAssignment.Application.Features.Roles.Queries.GetList;
using QrAssignment.Domain.Entity.App;

namespace QrAssignment.Application.Repositories
{
    public interface IAppRoleRepository 
    {
        Task<Paginate<RoleListItemDto>> GetDtoListAsync(PageRequestBaseDto request, CancellationToken ct = default);
        Task<Paginate<RoleListItemDto>> GetPassivedDtoListAsync(PageRequestBaseDto request, CancellationToken ct = default);
        Task<List<RoleListItemExcelDto>> GetExportListAsync(PageRequestBaseDto request, CancellationToken ct = default);
        Task<RoleItemDto?> GetDtoByIdAsync(Guid id, CancellationToken ct = default);
        Task<RoleItemDto?> GetPassivedDtoByIdAsync(Guid id, CancellationToken ct = default);
        Task BulkDelete(List<Guid> ids, CancellationToken ct); 
        Task<List<AppRole>> GetByNamesAsync(List<string> names, CancellationToken ct);
    }
}
 