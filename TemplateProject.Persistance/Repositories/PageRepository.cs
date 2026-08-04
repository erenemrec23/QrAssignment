using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Entity.App;
using QrAssignment.Persistance.Context;
using Microsoft.EntityFrameworkCore;
internal sealed class PageRepository : IPageRepository
{
    private readonly AppDbContext _context;
    public PageRepository(AppDbContext context) => _context = context;

    public Task<List<PageCatalogItemDto>> GetCatalogAsync(CancellationToken ct = default)
    => _context.Set<Page>()
        .AsNoTracking()
        .OrderBy(p => p.MenuGroupId).ThenBy(p => p.Order)
        .Select(p => new PageCatalogItemDto(
            p.PageKey,
            p.Key,
            p.MenuGroup != null ? p.MenuGroup.Key : null))   // grupsuz sayfa → null
        .ToListAsync(ct);
}