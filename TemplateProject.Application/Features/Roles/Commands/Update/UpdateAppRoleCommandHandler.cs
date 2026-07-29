using MediatR;
using Microsoft.AspNetCore.Identity;
using QrAssignment.Application.Features.Roles.Commands.Update;
using QrAssignment.Application.Interfaces;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Entity.App;   // AppRole, AppUser
using QrAssignment.Domain.Shared;
using System.Security.Claims;
 

public sealed class UpdateAppRoleCommandHandler : IRequestHandler<UpdateAppRoleCommand, Result>
{
    private readonly RoleManager<AppRole> _roleManager;
    private readonly IAppRoleRepository _appRoleRepository;
    private readonly IAppLocalizer _localizer;

    public UpdateAppRoleCommandHandler(RoleManager<AppRole> roleManager, 
        IAppRoleRepository appRoleRepository,
        IAppLocalizer localizer)
    {
        _roleManager = roleManager;
        _appRoleRepository = appRoleRepository;
        _localizer = localizer;
    }

    public async Task<Result> Handle(UpdateAppRoleCommand request, CancellationToken ct)
    {
        var role = await _roleManager.FindByIdAsync(request.Id.ToString());
        if (role is null)
            return Result.Failure(new Error(_localizer["Error.RoleNotFound"], "Error.RoleNotFound"));

        if (role.Name != request.Name && await _roleManager.RoleExistsAsync(request.Name))
            return Result.Failure(new Error(_localizer["Error.RoleDublicated"], "Error.RoleDublicated"));

        // 1) Personel — kendi AppUserRole join'in üzerinden (UserManager YOK)
        await _appRoleRepository.SyncAssignedUsersAsync(role.Id, request.UserIds, ct);

        // 2) Sayfa yetkileri — rol claim'leri
        var managedPages = request.Permissions.Select(p => p.PageName).ToHashSet();
        var existingClaims = await _roleManager.GetClaimsAsync(role);
        foreach (var claim in existingClaims.Where(c => managedPages.Contains(c.Type)))
            await _roleManager.RemoveClaimAsync(role, claim);
        foreach (var p in request.Permissions.Where(p => p.PermissionValue > 0))
            await _roleManager.AddClaimAsync(role, new Claim(p.PageName, p.PermissionValue.ToString()));

        // 3) Rol adı
        role.Name = request.Name;
        var updateResult = await _roleManager.UpdateAsync(role);
        if (!updateResult.Succeeded)
            return Result.Failure(new Error(
                 string.Format(_localizer["Error.RoleUCanNotUpdated"],
                 string.Join(", ", updateResult.Errors.Select(e => e.Description))), "Error.RoleUCanNotUpdated"));

        return Result.Success();
    }
}