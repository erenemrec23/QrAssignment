using MediatR;
using Microsoft.AspNetCore.Identity;
using QrAssignment.Application.Interfaces;
using QrAssignment.Application.Repositories;   // IAppUserRepository (rol atama join'i icin)
using QrAssignment.Application.Services;        // IPermissionSyncService
using QrAssignment.Domain.Entity.App;          // AppUser
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Users.Commands.Update
{
    internal sealed class UpdateAppUserCommandHandler : IRequestHandler<UpdateAppUserCommand, Result>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IAppUserRepository _appUserRepository;
        private readonly IPermissionSyncService _permissionSyncService;
        private readonly IAppLocalizer _localizer;

        public UpdateAppUserCommandHandler(
            UserManager<AppUser> userManager,
            IAppUserRepository appUserRepository,
            IPermissionSyncService permissionSyncService,
            IAppLocalizer localizer)
        {
            _userManager = userManager;
            _appUserRepository = appUserRepository;
            _permissionSyncService = permissionSyncService;
            _localizer = localizer;
        }

        public async Task<Result> Handle(UpdateAppUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.Id.ToString());
            if (user is null)
                return Result.Failure(new Error("Error.UserNotFound", _localizer["Error.UserNotFound"]));

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
                return Result.Failure(new Error(
                    "Error.UserCanNotUpdated",
                    string.Format(
                        _localizer["Error.UserCanNotUpdated"],
                        string.Join(", ", updateResult.Errors.Select(e => e.Description)))));

            // Yetkiler artik servis uzerinden (coka-cok imza; tek kullanici tek elemanli liste).
            if (request.Permissions is not null)
            {
                await _permissionSyncService.SyncUsersPermissionsAsync(
                    new[] { user.Id }, request.Permissions, cancellationToken);
            }

            // Rol atama (AppUserRole join) repository'de kaliyor.
            if (request.RoleIds is not null)
            {
                await _appUserRepository.SyncAssignedRolesAsync(
                    user.Id, request.RoleIds, cancellationToken);
            }

            return Result.Success();
        }
    }
}