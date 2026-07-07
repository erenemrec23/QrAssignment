using QrAssignment.Application.DTOs.List;
using QrAssignment.Application.Features.Tenants.Queries.GetById;
using QrAssignment.Application.Features.Tenants.Queries.GetList;
using QrAssignment.Application.Features.Tenants.Queries.GetListExportExcel;
using QrAssignment.Application.Interfaces;
using QrAssignment.Domain.Entity.App;

namespace QrAssignment.Application.Repositories
{
    public interface ITenantRepository : IGenericRepository<Tenant>
    {
        Task<Paginate<TenantListItemDto>> GetListAsync(PageRequestBaseDto request, CancellationToken cancellationToken);
        Task<TenantItemDto> GetById(Guid id, CancellationToken cancellationToken);
        Task<List<TenantListItemExcelDto>> GetExportListAsync(GetTenantListExportExcelQuery request, CancellationToken cancellationToken);

    }
}
