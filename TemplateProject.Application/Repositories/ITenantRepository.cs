using QrAssignment.Application.DTOs.List;
using QrAssignment.Application.Features.Tenants.DTOs;
using QrAssignment.Application.Features.Tenants.Queries.DTOs;
using QrAssignment.Application.Interfaces;
using QrAssignment.Domain.Entity.App;

namespace QrAssignment.Application.Repositories
{
    public interface ITenantRepository : IGenericRepository<Tenant>
    {

        Task<Tenant?> GetPassivedByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<Paginate<TenantListItemDto>> GetDtoListAsync(PageRequestBaseDto request, CancellationToken cancellationToken);
        Task<TenantItemDto> GetDtoByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<TenantItemDto> GetPassivedDtoByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<List<TenantListItemExcelDto>> GetExportListAsync(PageRequestBaseDto request, CancellationToken cancellationToken);
        Task BulkDelete(List<Guid> ids, CancellationToken cancellationToken);
        Task<Paginate<TenantListItemDto>> GetPassivedDtoListAsync(PageRequestBaseDto request, CancellationToken cancellationToken);
        Task<List<Tenant>> GetByRevNumsAsync(List<long> revnums, CancellationToken cancellationToken);
        Task<List<Tenant>> GetByNamesAsync(List<string> names, CancellationToken cancellationToken);

        Task BulkActiveByIds(List<Guid> ids, CancellationToken cancellationToken);
        Task SetActiveById(Guid id, CancellationToken cancellationToken);

        Task SetPassiveById(Guid id, CancellationToken cancellationToken);
        Task BulkSetPassiveByIds(List<Guid> ids, CancellationToken cancellationToken);

    }
}
