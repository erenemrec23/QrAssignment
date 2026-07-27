using MediatR;
using Microsoft.AspNetCore.Identity;
using QrAssignment.Application.Interfaces;
using QrAssignment.Domain.Shared; 
namespace QrAssignment.Application.Features.Roles.Commands.Delete
{
    // Handler
    public sealed class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, Result>
    {
        private readonly RoleManager<
 QrAssignment.Domain.Entity.App.AppRole> _roleManager;
        private readonly IAppLocalizer _localizer;

        public DeleteRoleCommandHandler(RoleManager<QrAssignment.Domain.Entity.App.AppRole> roleManager,
             IAppLocalizer localizer)
        {
            _roleManager = roleManager;
            _localizer = localizer;
        }

        public async Task<Result> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            var role = await _roleManager.FindByIdAsync(request.Id);
            if (role == null)
            {
                return Result.Failure(new Error(_localizer["Label.NoRecords"],""));
            }

            var result = await _roleManager.DeleteAsync(role);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description)); 
                return Result.Failure(new Error("Error.RoleNotFound", _localizer["Error.RoleNotFound"]));
            }

            return Result.Success();
        }
    }
}