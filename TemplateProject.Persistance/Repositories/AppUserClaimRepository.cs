using Microsoft.EntityFrameworkCore;
using QrAssignment.Application.Features.Permission.Queries.GetByUserId;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Entity.App;
using QrAssignment.Persistance.Context;

namespace QrAssignment.Persistance.Repositories;

public sealed class AppUserClaimRepository : IAppUserClaimRepository
{
    private readonly AppDbContext _context;

    public AppUserClaimRepository(AppDbContext context)
    {
        _context = context;
    }

    // Kullanıcının SADECE kendi satırları (PagePermission.UserId)
    public async Task<List<PermissionUserPageItemDto>> GetUserWithPermissionsAsync(
        Guid? userId, CancellationToken cancellationToken = default)
    {
        var rows = await _context.Set<PagePermission>()
            .AsNoTracking()
            .IgnoreQueryFilters()   // login anında ambient tenant set olmayabilir
            .Where(pp => pp.UserId == userId)
            .Select(pp => new { pp.Page.PageKey, pp.PermissionValue })
            .ToListAsync(cancellationToken);

        return rows.Select(r => new PermissionUserPageItemDto
        {
            PageName = r.PageKey,
            PermissionValue = (int)r.PermissionValue
        }).ToList();
    }

    // Kullanıcının KENDİ + ROLLERİNDEN gelen satırları, sayfa bazında bitwise OR
    public async Task<List<PermissionUserPageItemDto>> GetEffectivePagePermissionsAsync(
        Guid? userId, CancellationToken cancellationToken = default)
    {
        if (userId is null)
            return new();

        var pagePerms = _context.Set<PagePermission>()
            .AsNoTracking()
            .IgnoreQueryFilters();   // sahip zaten tenant'a bağlı; login'de filtreyi bypass

        // 1) Kendi satırları
        var userPerms = await pagePerms
            .Where(pp => pp.UserId == userId)
            .Select(pp => new { pp.Page.PageKey, pp.PermissionValue })
            .ToListAsync(cancellationToken);

        // 2) Rollerinin satırları
        var roleIds = _context.AppUserRole
            .Where(ur => ur.AppUserId == userId && ur.AppRoleId != null)
            .Select(ur => ur.AppRoleId!.Value);

        var rolePerms = await pagePerms
            .Where(pp => pp.RoleId != null && roleIds.Contains(pp.RoleId.Value))
            .Select(pp => new { pp.Page.PageKey, pp.PermissionValue })
            .ToListAsync(cancellationToken);

        // 3) Sayfa bazında OR ile merge
        return userPerms.Concat(rolePerms)
            .GroupBy(x => x.PageKey)
            .Select(g => new PermissionUserPageItemDto
            {
                PageName = g.Key,
                PermissionValue = g.Aggregate(0, (acc, x) => acc | (int)x.PermissionValue)
            })
            .Where(dto => dto.PermissionValue > 0)
            .ToList();
    }
}