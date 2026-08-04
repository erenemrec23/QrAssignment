using MediatR;
using Microsoft.AspNetCore.Identity;
using QrAssignment.Application.Abstractions;
using QrAssignment.Application.Interfaces;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Entity.App;
using QrAssignment.Domain.Shared;
using QrAssignment.Domain.Shared.PagePermission;

namespace QrAssignment.Application.Features.Roles.Commands.Create
{
    public sealed class CreateAppRoleCommandHandler : IRequestHandler<CreateAppRoleCommand, Result>
    {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IAppRoleRepository _appRoleRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAppLocalizer _localizer;

        public CreateAppRoleCommandHandler(
            RoleManager<AppRole> roleManager,
            IAppRoleRepository appRoleRepository,
            IUnitOfWork unitOfWork,
            IAppLocalizer localizer)
        {
            _roleManager = roleManager;
            _appRoleRepository = appRoleRepository;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result> Handle(CreateAppRoleCommand request, CancellationToken ct)
        {
            if (await _roleManager.RoleExistsAsync(request.Name))
                return Result.Failure(new Error(
                    "Error.RoleHasInserted",
                    string.Format(_localizer["Error.RoleHasInserted"], request.Name)));

            var role = new AppRole { Name = request.Name.Trim() };

            var createResult = await _roleManager.CreateAsync(role);
            if (!createResult.Succeeded)
            {
                var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                return Result.Failure(new Error(
                    string.Format(_localizer["Error.CreateRole"], errors), ""));
            }

            await _unitOfWork.SaveChangesAsync(ct);   // role.Id üretilsin

            if (request.UserIds is { Count: > 0 })
                await _appRoleRepository.SyncAssignedUsersAsync(role.Id, request.UserIds, ct);

            // Sayfa yetkileri artık PagePermission tablosuna (Identity claim değil)
            await _appRoleRepository.SyncRolePermissionsAsync(role.Id, request.Permissions, PermissionTargetScope.Page, ct);

            return Result.Success();
        }
    }
}