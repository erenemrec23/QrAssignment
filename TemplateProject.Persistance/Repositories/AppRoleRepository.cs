using DocumentFormat.OpenXml.Drawing.Charts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QrAssignment.Application.DTOs.List;
using QrAssignment.Application.Features.Permission.Queries.GetByUserId;
using QrAssignment.Application.Features.Roles.Commands.Update;
using QrAssignment.Application.Features.Roles.DTOs;
using QrAssignment.Application.Features.Roles.Queries.GetList;
using QrAssignment.Application.Repositories;
using QrAssignment.Application.Services;
using QrAssignment.Domain.Entity.App;
using QrAssignment.Domain.Shared;
using QrAssignment.Persistance.Context;
using System.Linq.Expressions;

namespace QrAssignment.Persistance.Repositories;
internal sealed class AppRoleRepository : GenericAppRepository<AppRole>, IAppRoleRepository
{
    private readonly ITenantIdService _tenantIdService;
    public AppRoleRepository(AppDbContext context,
         ITenantIdService tenantIdService) : base(context)
    {
        _tenantIdService = tenantIdService;
    }

    private static Expression<Func<AppRole, RoleListItemDto>> ProjectionList =>
        r => new RoleListItemDto(r.Id,
            r.Name!,
            r.RevNum,
            r.ModifiedByUser != null ? r.ModifiedByUser.FullName : "",
            r.CreatedByUser != null ? r.CreatedByUser.FullName : "",
            r.ModifiedDate,
            r.CreatedDate);
    private static Expression<Func<AppRole, RoleItemDto>> ProjectionItem =>
        r => new RoleItemDto(r.Id, r.Name!, r.RowVersion);
    private static Expression<Func<AppRole, RoleListItemExcelDto>> ProjectionExcelItem =>
        r => new RoleListItemExcelDto(r.Name!);



    public Task<Paginate<RoleListItemDto>> GetDtoListAsync(PageRequestBaseDto request, CancellationToken ct = default)
        => PaginateAsync(ProjectionList, request, ct);

    public Task<Paginate<RoleListItemDto>> GetPassivedDtoListAsync(PageRequestBaseDto request, CancellationToken ct = default)
        => PaginatePassivedAsync(ProjectionList, request, ct);

    public Task<List<RoleListItemExcelDto>> GetExportListAsync(PageRequestBaseDto request, CancellationToken ct = default)
        => ListAsync(ProjectionExcelItem, request, ct);

    public Task<RoleItemDto?> GetDtoByIdAsync(Guid id, CancellationToken ct = default)
        => SingleDtoByIdAsync(id, ProjectionItem, ct);

    public Task<RoleItemDto?> GetPassivedDtoByIdAsync(Guid id, CancellationToken ct = default)
        => SinglePassivedDtoByIdAsync(id, ProjectionItem, ct);
    public Task<AppRole?> GetPassivedByIdAsync(Guid id, CancellationToken ct = default)
        => SinglePassivedByIdAsync(id, ct);

    public Task BulkDelete(List<Guid> ids, CancellationToken ct)
        => BulkDeleteByIdsAsync(ids, ct);
    public Task DeleteById(Guid id, CancellationToken ct)
        => DeleteByIdAsync(id, ct);


    public Task SetPassiveById(Guid id, CancellationToken ct)
        => SetPassiveByIdAsync(id, ct);
    public Task BulkSetPassiveByIds(List<Guid> ids, CancellationToken ct)
        => BulkSetPassiveByIdsAsync(ids, ct);

    public Task Delete(Guid id, CancellationToken ct)
        => DeleteByIdAsync(id, ct);

    public Task<List<AppRole>> GetByNamesAsync(List<string> names, CancellationToken ct)
    => GetByValuesAsync(r => r.Name!, names, ct);


    public async Task<List<Guid>> GetAssignedUserListDtoAsync(Guid roleId, CancellationToken ct = default)
    {

        var result = await _context.AppUserRole
            .Where(ur => ur.AppRoleId == roleId)
            .Join(_context.AppUsers,
                  ur => ur.AppUserId,
                  u => u.Id,
                  (ur, u) => u.Id)
            .ToListAsync(ct);

        return result;

    }
    public async Task SyncAssignedUsersAsync(Guid roleId, IEnumerable<Guid> userIds, CancellationToken ct)
    {
        // Olması gereken kullanıcılar (formdan gelen)
        var target = userIds?.ToHashSet() ?? new HashSet<Guid>();

        // DB'de şu an bu role atanmış satırlar
        var current = await _context.AppUserRole
            .Where(ur => ur.AppRoleId == roleId)
            .ToListAsync(ct);

        var currentIds = current
            .Where(ur => ur.AppUserId.HasValue)
            .Select(ur => ur.AppUserId!.Value)
            .ToHashSet();

        // 1) DB'de var ama formda YOK → çıkar
        var toRemove = current.Where(ur => ur.AppUserId.HasValue
                                        && !target.Contains(ur.AppUserId.Value));
        _context.AppUserRole.RemoveRange(toRemove);

        // 2) Formda var ama DB'de YOK → ekle
        var toAdd = target
            .Where(id => !currentIds.Contains(id))
            .Select(id => new AppUserRole { AppRoleId = roleId, AppUserId = id });
        await _context.AppUserRole.AddRangeAsync(toAdd, ct);

        // 3) İkisinde de olanlara dokunulmuyor → mevcut satır ve audit alanları korunur

        // SaveChanges YOK — UnitOfWorkBehavior commit edecek
    }

    public async Task<List<PermissionUserPageItemDto>> GetAssignedPermissionListDtoAsync(
    Guid roleId, CancellationToken cancellationToken)
    {
        var rows = await _context.Set<PagePermission>()
            .AsNoTracking()
            .Where(pp => pp.RoleId == roleId)
            .Select(pp => new { pp.Page.PageKey, pp.PermissionValue })
            .ToListAsync(cancellationToken);

        return rows.Select(r => new PermissionUserPageItemDto
        {
            PageName = r.PageKey,
            PermissionValue = (int)r.PermissionValue
        }).ToList();
    }


    public Task BulkSetActiveAsync(List<Guid> ids, CancellationToken ct)
        => BulkSetActiveByIdsAsync(ids, ct);


    public Task SetActiveAsync(Guid id, CancellationToken ct)
        => SetActiveByIdAsync(id, ct);

    public async Task SyncRolePermissionsAsync(
    Guid roleId, IEnumerable<RolePagePermissionDto> permissions, CancellationToken ct = default)
    {
        // Gelen yetkiler: sadece PermissionValue > 0 olanları tut (0 = yetki yok = satır olmasın)
        var incoming = (permissions ?? [])
            .Where(p => p.PermissionValue > 0)
            .ToList();

        // PageKey → PageId çevirisi (string ClaimType yerine artık gerçek FK)
        var pageKeys = incoming.Select(p => p.PageName).ToHashSet();
        var pageMap = await _context.Set<Page>()
            .Where(pg => pageKeys.Contains(pg.PageKey))
            .Select(pg => new { pg.Id, pg.PageKey })
            .ToDictionaryAsync(x => x.PageKey, x => x.Id, ct);

        // Rolün mevcut satırları
        var current = await _context.Set<PagePermission>()
            .Where(pp => pp.RoleId == roleId)
            .ToListAsync(ct);

        var tenantId = _tenantIdService.GetTenantId();   // rol tenant-scoped; satır da aynı tenant'ta

        foreach (var p in incoming)
        {
            if (!pageMap.TryGetValue(p.PageName, out var pageId))
                continue;   // bilinmeyen sayfa key'i — sessizce atla (istersen burada hata fırlat)

            var existing = current.FirstOrDefault(x => x.PageId == pageId);
            if (existing is null)
            {
                _context.Set<PagePermission>().Add(
                    PagePermission.ForRole(roleId, pageId, (PagePermissions)p.PermissionValue, tenantId));
            }
            else
            {
                existing.PermissionValue = (PagePermissions)p.PermissionValue;   // güncelle
            }
        }

        // Gelen listede artık olmayan sayfalar → sil
        var incomingPageIds = incoming
            .Where(p => pageMap.ContainsKey(p.PageName))
            .Select(p => pageMap[p.PageName])
            .ToHashSet();

        var toRemove = current.Where(x => !incomingPageIds.Contains(x.PageId));
        _context.Set<PagePermission>().RemoveRange(toRemove);

        // SaveChanges YOK — UnitOfWorkBehavior commit edecek (SyncAssignedUsersAsync ile aynı desen)
    }
}
