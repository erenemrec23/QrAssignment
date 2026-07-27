using MediatR;
using Microsoft.AspNetCore.Identity;
using QrAssignment.Application.Features.Roles.Commands.Create;
using QrAssignment.Application.Interfaces;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Roles.Commands.Create
{
    // Handler
    public sealed class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, Result>
    {
        private readonly RoleManager<QrAssignment.Domain.Entity.App.AppRole> _roleManager;
        private readonly IAppLocalizer _localizer;

        public CreateRoleCommandHandler(RoleManager<QrAssignment.Domain.Entity.App.AppRole> roleManager,
            IAppLocalizer localizer)
        {
            _roleManager = roleManager;
            _localizer = localizer;
        }

        public async Task<Result> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            var roleExists = await _roleManager.RoleExistsAsync(request.Name);
            if (roleExists)
            {
                return Result.Failure(new Error("Error.RoleHasInserted", string.Format(_localizer["Error.RoleHasInserted"], request.Name)));
            }

            var appRole = new QrAssignment.Domain.Entity.App.AppRole
            {
                Name = request.Name.Trim(),  
            };

            var result = await _roleManager.CreateAsync(appRole);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return Result.Failure(new Error(string.Format(_localizer["Error.CreateRole"],errors), ""));
            }

            return Result.Success();
        }
    }
}