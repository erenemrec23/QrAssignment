using QrAssignment.Application.Abstractions;
using QrAssignment.Domain.Shared;
using QrAssignment.Domain.Shared.PagePermission;

namespace QrAssignment.Application.Features.Permission.Commands.Update
{
    public sealed record UpdateUserPermissionCommand(
    string UserId,
    List<PermissionUserUpdateDto> Permissions,
    PermissionTargetScope Scope = PermissionTargetScope.Page) : ICommand<Result>;
}
