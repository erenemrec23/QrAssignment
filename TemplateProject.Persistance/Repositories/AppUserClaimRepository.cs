using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QrAssignment.Application.Features.Permission.Queries.GetByUserId;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Entity.App;

namespace QrAssignment.Persistance.Repositories;

public sealed class AppUserClaimRepository : IAppUserClaimRepository
{
    private readonly UserManager<AppUser> _userManager;

    public AppUserClaimRepository(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }


    public async Task<List<PermissionUserPageItemDto>> GetUserWithPermissionsAsync(Guid? userId, CancellationToken cancellationToken = default)
    {
       
        var rawClaims = await _userManager.Users
            .AsNoTracking()
            .IgnoreQueryFilters()
            .Where(u => u.Id == userId)
            .SelectMany(u => u.Claims)  
            .Where(c => c.ClaimType.StartsWith("Page_"))  
            .Select(c => new
            {
                c.ClaimType,
                c.ClaimValue
            })
            .ToListAsync(cancellationToken);
         
        var dtoList = rawClaims.Select(c => new PermissionUserPageItemDto
        {
            PageName = c.ClaimType,
            PermissionValue = int.TryParse(c.ClaimValue, out int val) ? val : 0
        }).ToList();
        return dtoList;
    }
}