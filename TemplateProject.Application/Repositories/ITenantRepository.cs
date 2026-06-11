using QrAssignment.Application.Features.Tenants.Queries.GetList;
using QrAssignment.Application.Interfaces;
using QrAssignment.Domain.Entity.App;

namespace QrAssignment.Application.Repositories
{
    public interface ITenantRepository : IGenericRepository<Tenant>
    {
        Task<List<TenantListItemDto>> GetList(CancellationToken cancellationToken);
        Task<TenantItemDto> GetById(Guid id, CancellationToken cancellationToken);

    }
}
