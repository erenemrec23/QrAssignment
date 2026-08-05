using QrAssignment.Application.Interfaces;
using QrAssignment.Domain.Entity.App;

namespace QrAssignment.Application.Repositories
{
    public interface IPagePermissionRepository : IGenericRepository<PagePermission>
    {
        Task<List<PagePermission>> GetPagePermissionList(int pageId, CancellationToken ct = default);
    }
}
 