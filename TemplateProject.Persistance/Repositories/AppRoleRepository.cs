using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QrAssignment.Application.Features.AppRole.Queries.GetList;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Entity.App;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Persistance.Repositories;

public sealed class AppRoleRepository : IAppRoleRepository
{
    private readonly RoleManager<AppRole> _roleManager;

    public AppRoleRepository(RoleManager<AppRole> roleManager)
    {
        _roleManager = roleManager;
    }
    public async Task<List<AppRoleListItemDto>> GetList(CancellationToken cancellationToken)
    {
        // RoleManager.Roles bize IQueryable<AppRole> döner, doğrudan SQL tarafında filtreleriz
        var roles = await _roleManager.Roles
            .Select(r => new AppRoleListItemDto(r.Id.ToString(), r.Name!)) // Guid ise ToString() kullanabilirsin
            .ToListAsync(cancellationToken);

        return roles;
    }
}
