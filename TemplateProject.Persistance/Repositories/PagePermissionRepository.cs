using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Entity.App;
using QrAssignment.Persistance.Context;
using QrAssignment.Persistance.Repositories.Base;
using System.Data.Entity;

internal sealed class PagePermissionRepository : GenericRepository<PagePermission>, IPagePermissionRepository
{

    private readonly AppDbContext _context;
    public PagePermissionRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public Task<List<PagePermission>> GetPagePermissionList(int pageId, CancellationToken ct = default)
    => _context.Set<PagePermission>().Where(p => p.PageId == pageId).ToListAsync(ct); 
}
