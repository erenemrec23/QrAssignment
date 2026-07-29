using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QrAssignment.Application.Features.Permission.Queries.GetByUserId;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Entity.App;
using QrAssignment.Persistance.Context;

namespace QrAssignment.Persistance.Repositories;

public sealed class AppUserClaimRepository : IAppUserClaimRepository
{
    private readonly UserManager<AppUser> _userManager;
    private readonly AppDbContext _context;

    public AppUserClaimRepository(UserManager<AppUser> userManager, AppDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    // Mevcut metod aynı kalıyor — sadece kullanıcının KENDİ claim'leri
    public async Task<List<PermissionUserPageItemDto>> GetUserWithPermissionsAsync(
        Guid? userId, CancellationToken cancellationToken = default)
    {
        var rawClaims = await _userManager.Users
            .AsNoTracking()
            .IgnoreQueryFilters()
            .Where(u => u.Id == userId)
            .SelectMany(u => u.Claims)
            .Where(c => c.ClaimType.StartsWith("Page_"))
            .Select(c => new { c.ClaimType, c.ClaimValue })
            .ToListAsync(cancellationToken);

        return rawClaims.Select(c => new PermissionUserPageItemDto
        {
            PageName = c.ClaimType,
            PermissionValue = int.TryParse(c.ClaimValue, out var val) ? val : 0
        }).ToList();
    }

    // YENİ — kullanıcı claim'leri + rol claim'leri, sayfa bazında OR ile merge
    public async Task<List<PermissionUserPageItemDto>> GetEffectivePagePermissionsAsync(
        Guid? userId, CancellationToken cancellationToken = default)
    {
        if (userId is null)
            return new();

        // 1) Kullanıcının kendi sayfa claim'leri
        var userClaims = await _userManager.Users
            .AsNoTracking()
            .IgnoreQueryFilters()
            .Where(u => u.Id == userId)
            .SelectMany(u => u.Claims)
            .Where(c => c.ClaimType.StartsWith("Page_"))
            .Select(c => new { c.ClaimType, c.ClaimValue })
            .ToListAsync(cancellationToken);

        // 2) Kullanıcının atandığı rollerin sayfa claim'leri (AppUserRole join → RoleClaims)
        var roleClaims = await (
            from ur in _context.AppUserRole.AsNoTracking()
            where ur.AppUserId == userId && ur.AppRoleId != null
            join rc in _context.Set<IdentityRoleClaim<Guid>>().AsNoTracking()
                on ur.AppRoleId!.Value equals rc.RoleId
            where rc.ClaimType.StartsWith("Page_")
            select new { rc.ClaimType, rc.ClaimValue }
        ).ToListAsync(cancellationToken);

        // 3) Merge — aynı sayfa için tüm değerleri bitwise OR ile birleştir
        var merged = userClaims.Concat(roleClaims)
            .Where(c => !string.IsNullOrEmpty(c.ClaimType))
            .GroupBy(c => c.ClaimType)
            .Select(g => new PermissionUserPageItemDto
            {
                PageName = g.Key,
                PermissionValue = g.Aggregate(0, (acc, c) =>
                    acc | (int.TryParse(c.ClaimValue, out var v) ? v : 0))
            })
            .Where(dto => dto.PermissionValue > 0)   // hiç bit yoksa taşıma
            .ToList();

        return merged;
    }
}