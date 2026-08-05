using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QrAssignment.Application.DTOs.List;
using QrAssignment.Application.Features.Permission.Commands.Update;
using QrAssignment.Application.Features.Permission.Queries.GetByUserId;
using QrAssignment.Application.Features.Users.DTOs;
using QrAssignment.Application.Features.Users.Queries.DTOs;
using QrAssignment.Application.Features.Users.Queries.LookUp.DTOs;
using QrAssignment.Application.Repositories;
using QrAssignment.Application.Services;
using QrAssignment.Domain.Entity.App;
using QrAssignment.Domain.Shared.PagePermission;
using QrAssignment.Persistance.Context;
using System.Linq.Expressions;
using TemplateProject.Persistance.Migrations;

namespace QrAssignment.Persistance.Repositories;

internal sealed class AppUserRepository : GenericAppRepository<AppUser>, IAppUserRepository
{
    ITenantIdService _tenantIdService;
    public AppUserRepository(AppDbContext context, ITenantIdService tenantIdService) : base(context)
    {
        _tenantIdService = tenantIdService;
    }

    private static Expression<Func<AppUser, AppUserListItemDto>> ProjectionList =>
        u => new AppUserListItemDto(
            u.Id,
            u.FirstName,
            u.LastName,
            u.FullName,
            u.Email!,
            u.RevNum,
            u.ModifiedByUser != null ? u.ModifiedByUser.FullName : "",
            u.CreatedByUser != null ? u.CreatedByUser.FullName : "",
            u.ModifiedDate,
            u.CreatedDate);

    private static Expression<Func<AppUser, AppUserItemDto>> ProjectionItem =>
        u => new AppUserItemDto(u.Id, u.FirstName, u.LastName, u.Email!, u.RowVersion);

    private static Expression<Func<AppUser, AppUserListItemExcelDto>> ProjectionExcelItem =>
        u => new AppUserListItemExcelDto(u.FullName, u.Email!);

    // --- Ortak okuma yuzeyi ---
    public Task<Paginate<AppUserListItemDto>> GetDtoListAsync(PageRequestBaseDto request, CancellationToken ct = default)
        => PaginateAsync(ProjectionList, request, ct);

    public Task<Paginate<AppUserListItemDto>> GetPassivedDtoListAsync(PageRequestBaseDto request, CancellationToken ct = default)
        => PaginatePassivedAsync(ProjectionList, request, ct);

    public Task<List<AppUserListItemExcelDto>> GetExportListAsync(PageRequestBaseDto request, CancellationToken ct = default)
        => ListAsync(ProjectionExcelItem, request, ct);

    public Task<AppUserItemDto?> GetDtoByIdAsync(Guid id, CancellationToken ct = default)
        => SingleDtoByIdAsync(id, ProjectionItem, ct);

    public Task<AppUserItemDto?> GetPassivedDtoByIdAsync(Guid id, CancellationToken ct = default)
        => SinglePassivedDtoByIdAsync(id, ProjectionItem, ct);
    public Task<AppUser?> GetPassivedByIdAsync(Guid id, CancellationToken ct = default)
        => SinglePassivedByIdAsync(id, ct);

    public Task BulkDeleteAsync(List<Guid> ids, CancellationToken ct)
        => BulkDeleteByIdsAsync(ids, ct);


    public Task BulkSetActiveByIds(List<Guid> ids, CancellationToken ct)
        => BulkSetActiveByIdsAsync(ids, ct);


    public Task SetActiveAsync(Guid id, CancellationToken ct)
        => SetActiveByIdAsync(id, ct);
    public Task DeleteById(Guid id, CancellationToken ct)
        => DeleteByIdAsync(id, ct);

    // --- Excel Bulk Validation Helpers ---
    public async Task<List<string>> GetExistingUserNamesAsync(List<string> userNames, CancellationToken ct = default)
    {
        var users = await GetByValuesAsync(u => u.UserName!, userNames, ct);
        return users.Select(u => u.UserName!).ToList();
    }

    public async Task<List<string>> GetExistingEmailsAsync(List<string> emails, CancellationToken ct = default)
    {
        var users = await GetByValuesAsync(u => u.Email!, emails, ct);
        return users.Select(u => u.Email!).ToList();
    }

    // --- User'a ozel ---
    public Task<List<AppUserLookUpListItemDto>> GetLookUpList(CancellationToken ct)
        => _context.AppUsers
            .AsNoTracking()
            .Select(u => new AppUserLookUpListItemDto { Id = u.Id, FullName = u.FullName })
            .ToListAsync(ct);

    public Task<AppUser?> GetByIdWithRefreshTokenAsync(Guid id, CancellationToken ct = default)
        => _context.AppUsers
            .Include(u => u.RefreshToken)
            .FirstOrDefaultAsync(u => u.Id == id, ct);

    public Task<AppUser?> GetByEmailWithRefreshTokenAsync(string email, CancellationToken ct = default)
        => _context.AppUsers
            .Include(u => u.RefreshToken)
            .Include(u => u.AppUserRoles)
            .ThenInclude(ur => ur.AppRole)
            .AsNoTracking()
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(u => u.Email == email, ct);


    public Task<AppUser?> GetByEmailForRememberPasswordAsync(string email, CancellationToken ct = default)
        => _context.AppUsers
            .AsNoTracking()
            .IgnoreQueryFilters(["TenantFilter"])
            .FirstOrDefaultAsync(u => u.Email == email, ct);

    // --- Role Sync & Permission Mappings ---
    public async Task<List<Guid>> GetAssignedRoleListDtoAsync(Guid userId, CancellationToken ct = default)
    {
        return await _context.AppUserRole
            .Where(ur => ur.AppUserId == userId && ur.AppRoleId.HasValue)
            .Select(ur => ur.AppRoleId!.Value)
            .ToListAsync(ct);
    }

    public async Task SyncAssignedRolesAsync(Guid userId, IEnumerable<Guid> roleIds, CancellationToken ct = default)
    {
        var target = roleIds?.ToHashSet() ?? new HashSet<Guid>();

        var current = await _context.AppUserRole
            .Where(ur => ur.AppUserId == userId)
            .ToListAsync(ct);

        var currentRoleIds = current
            .Where(ur => ur.AppRoleId.HasValue)
            .Select(ur => ur.AppRoleId!.Value)
            .ToHashSet();

        // 1) DB'de var ama formda YOK -> sil
        var toRemove = current.Where(ur => ur.AppRoleId.HasValue && !target.Contains(ur.AppRoleId.Value));
        _context.AppUserRole.RemoveRange(toRemove);

        // 2) Formda var ama DB'de YOK -> ekle
        var toAdd = target
            .Where(id => !currentRoleIds.Contains(id))
            .Select(id => new AppUserRole { AppUserId = userId, AppRoleId = id });

        await _context.AppUserRole.AddRangeAsync(toAdd, ct);
    }

    public async Task<List<PermissionUserPageItemDto>> GetAssignedPermissionListDtoAsync(
    Guid userId, CancellationToken cancellationToken = default)
    {
        var roleIds = _context.AppUserRole
            .Where(ur => ur.AppUserId == userId && ur.AppRoleId != null)
            .Select(ur => ur.AppRoleId!.Value);

        var rows = await _context.Set<PagePermission>()
            .AsNoTracking()
            .Where(pp => pp.RoleId != null && roleIds.Contains(pp.RoleId.Value))
            .Select(pp => new { pp.Page.PageKey, pp.PermissionValue })
            .ToListAsync(cancellationToken);

        return rows
            .GroupBy(r => r.PageKey)
            .Select(g => new PermissionUserPageItemDto
            {
                PageName = g.Key,
                PermissionValue = g.Aggregate(0, (acc, r) => acc | (int)r.PermissionValue)
            })
            .ToList();
    }

    public async Task SyncUserPermissionsAsync(
    Guid userId, IEnumerable<PermissionUserUpdateDto> permissions, CancellationToken ct = default)
    {
        var incoming = (permissions ?? []).Where(p => p.PermissionValue > 0).ToList();
        var tenantId = _tenantIdService.GetTenantId();

        // Hedef tipine göre ayır — Scope alanına gerek yok, PageName/GroupKey belli ediyor
        var pageItems = incoming.Where(p => !string.IsNullOrEmpty(p.PageName)).ToList();
        var groupItems = incoming.Where(p => string.IsNullOrEmpty(p.PageName) && !string.IsNullOrEmpty(p.GroupKey)).ToList();

        await SyncPageRowsAsync(userId, pageItems, tenantId, ct);
        await SyncGroupRowsAsync(userId, groupItems, tenantId, ct);
        // SaveChanges YOK — UnitOfWorkBehavior tek transaction'da commit eder
    }

    // --- SAYFA satırları ---
    private async Task SyncPageRowsAsync(Guid userId, List<PermissionUserUpdateDto> items, Guid? tenantId, CancellationToken ct)
    {
        var keys = items.Select(p => p.PageName!).ToHashSet();
        var map = await _context.Set<Page>()
            .IgnoreQueryFilters(["SoftDeleteFilter"])
            .Where(pg => keys.Contains(pg.PageKey))
            .Select(pg => new { pg.Id, pg.PageKey })
            .ToDictionaryAsync(x => x.PageKey, x => x.Id, ct);

        var current = await _context.Set<PagePermission>()
            .IgnoreQueryFilters(["SoftDeleteFilter"])
            .Where(pp => pp.UserId == userId && pp.PageId != null)   // yalnızca SAYFA satırları
            .ToListAsync(ct);

        foreach (var p in items)
        {
            if (!map.TryGetValue(p.PageName!, out var pageId)) continue;
            var existing = current.FirstOrDefault(x => x.PageId == pageId);
            if (existing is null)
                _context.Set<PagePermission>().Add(
                    PagePermission.ForUser(userId, pageId, (PageAccessFlags)p.PermissionValue, tenantId));
            else
            {

                existing.PermissionValue = (PageAccessFlags)p.PermissionValue;
                existing.IsPassived = false; 
            }
        }

        var ids = items.Where(p => map.ContainsKey(p.PageName!)).Select(p => map[p.PageName!]).ToHashSet();
        _context.Set<PagePermission>().RemoveRange(current.Where(x => !ids.Contains(x.PageId!.Value)));
    }

    // --- GRUP satırları ---
    private async Task SyncGroupRowsAsync(Guid userId, List<PermissionUserUpdateDto> items, Guid? tenantId, CancellationToken ct)
    {
        var keys = items.Select(p => p.GroupKey!).ToHashSet();
        var map = await _context.Set<MenuGroup>()
            .IgnoreQueryFilters(["SoftDeleteFilter"])
            .Where(g => keys.Contains(g.Key))
            .Select(g => new { g.Id, g.Key })
            .ToDictionaryAsync(x => x.Key, x => x.Id, ct);

        var current = await _context.Set<PagePermission>()
            .IgnoreQueryFilters(["SoftDeleteFilter"])
            .Where(pp => pp.UserId == userId && pp.MenuGroupId != null)   // yalnızca GRUP satırları
            .ToListAsync(ct);

        foreach (var p in items)
        {
            if (!map.TryGetValue(p.GroupKey!, out var groupId)) continue;
            var existing = current.FirstOrDefault(x => x.MenuGroupId == groupId);
            if (existing is null)
                _context.Set<PagePermission>().Add(
                    PagePermission.ForUserGroup(userId, groupId, (PageAccessFlags)p.PermissionValue, tenantId));
            else
            { 
                existing.PermissionValue = (PageAccessFlags)p.PermissionValue;
                existing.IsPassived = false;
            }
        }

        var ids = items.Where(p => map.ContainsKey(p.GroupKey!)).Select(p => map[p.GroupKey!]).ToHashSet();
        _context.Set<PagePermission>().RemoveRange(current.Where(x => !ids.Contains(x.MenuGroupId!.Value)));
    }
}