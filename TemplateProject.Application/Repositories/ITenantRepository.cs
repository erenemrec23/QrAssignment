using QrAssignment.Application.DTOs.List;
using QrAssignment.Application.Features.Tenants.Commands.Excel.BulkCreate;
using QrAssignment.Application.Features.Tenants.DTOs; 
using QrAssignment.Application.Features.Tenants.Queries.GetListExportExcel;
using QrAssignment.Application.Interfaces;
using QrAssignment.Domain.Entity.App;

namespace QrAssignment.Application.Repositories
{
    public interface ITenantRepository : IGenericRepository<Tenant>
    {
        Task<Paginate<TenantListItemDto>> GetDtoListAsync(PageRequestBaseDto request, CancellationToken cancellationToken);
        Task<TenantItemDto> GetDtoByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<TenantItemDto> GetPassivedDtoByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<List<TenantListItemExcelDto>> GetExportListAsync(GetTenantListExportExcelQuery request, CancellationToken cancellationToken);

        Task BulkDelete(List<Guid> ids, CancellationToken cancellationToken);

        Task<Paginate<TenantListItemDto>> GetPassivedDtoListAsync(PageRequestBaseDto request, CancellationToken cancellationToken);
        Task<List<string>> GetDublicateDataList(List<CreateTenantInputDto> datas, CancellationToken cancellationToken);
    }
}
