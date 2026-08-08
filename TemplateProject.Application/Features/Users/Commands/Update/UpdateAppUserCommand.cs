using QrAssignment.Application.Abstractions;
using QrAssignment.Application.Features.Permission.Commands.Update; // PermissionUserUpdateDto
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Users.Commands.Update
{
    public sealed record UpdateAppUserCommand(
        Guid? Id,
        string FirstName,
        string LastName,
        // Sayfa/grup yetkileri (null => dokunma; boş liste => tüm sayfa yetkilerini sıfırla)
        List<PermissionUserUpdateDto>? Permissions = null,
        // Atanacak roller (null => dokunma; boş liste => tüm rolleri kaldır)
        List<Guid>? RoleIds = null) : ICommand<Result>, IdValidationBase;
}