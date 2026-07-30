using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QrAssignment.Application.DTOs.List;
using QrAssignment.Application.Features.Permission.Queries.GetByUserId;
using QrAssignment.Application.Features.Users.DTOs;
using QrAssignment.Application.Features.Users.Queries.DTOs;
using QrAssignment.Application.Features.Users.Queries.LookUp.DTOs;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Entity.App;
using QrAssignment.Persistance.Context;
using System.Linq.Expressions;

namespace QrAssignment.Persistance.Repositories;

internal sealed class AppUserRepository : GenericAppRepository<AppUser>, IAppUserRepository
{
    public AppUserRepository(AppDbContext context) : base(context) { }

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

    public async Task<List<PermissionUserPageItemDto>> GetAssignedPermissionListDtoAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        // Kullanıcının doğrudan sahip olduğu Role ID'lerini çekiyoruz
        var userRoleIds = await _context.AppUserRole
            .Where(ur => ur.AppUserId == userId && ur.AppRoleId.HasValue)
            .Select(ur => ur.AppRoleId!.Value)
            .ToListAsync(cancellationToken);

        if (!userRoleIds.Any())
            return new List<PermissionUserPageItemDto>();

        // Bu rollerin IdentityRoleClaim tablosundaki izinlerini gruplayıp getiriyoruz
        var claims = await _context.Set<IdentityRoleClaim<Guid>>()
            .AsNoTracking()
            .Where(rc => userRoleIds.Contains(rc.RoleId))
            .Select(rc => new { rc.ClaimType, rc.ClaimValue })
            .ToListAsync(cancellationToken);

        return claims
            .Where(c => !string.IsNullOrEmpty(c.ClaimType))
            .GroupBy(c => c.ClaimType!)
            .Select(g => new PermissionUserPageItemDto
            {
                PageName = g.Key,
                // Birden fazla rolden gelen bitmask değerlerini OR (bitwise) işlemi ile birleştiririz
                PermissionValue = g.Aggregate(0, (acc, c) => acc | (int.TryParse(c.ClaimValue, out var v) ? v : 0))
            })
            .ToList();
    }
}