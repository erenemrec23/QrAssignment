using MediatR;
using Microsoft.AspNetCore.Identity; 
using QrAssignment.Application.Interfaces;
using QrAssignment.Domain.Entity.App;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Roles.Commands.SetActive
{
    internal sealed class SetPassiveAppRoleCommandHandler : IRequestHandler<SetActiveAppRoleCommand, Result>
    {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IAppLocalizer _localizer;

        public SetPassiveAppRoleCommandHandler(RoleManager<AppRole> roleManager, IAppLocalizer localizer)
        {
            _roleManager = roleManager;
            _localizer = localizer;
        }

        public async Task<Result> Handle(SetActiveAppRoleCommand request, CancellationToken cancellationToken)
        {
            var role = await _roleManager.FindByIdAsync(request.Id.ToString()!);
            if (role is null)
                return Result.Failure(new Error("Error.RoleNotFound", _localizer["Error.RoleNotFound"]));

            role.IsPassived = false;

            var updateResult = await _roleManager.UpdateAsync(role);
            if (!updateResult.Succeeded)
                return Result.Failure(new Error(
                    "Error.RoleCanNotUpdated",
                    string.Format(
                        _localizer["Error.RoleCanNotUpdated"],
                        string.Join(", ", updateResult.Errors.Select(e => e.Description)))));

            return Result.Success();
        }
    }
}