namespace QrAssignment.Application.Features.PagePermissions.DTOs
{
    public sealed record PagePermissionAssignmentDto(
       Guid? UserId,
       string? UserName,
       Guid? RoleId,
       string? RoleName,
       int PermissionValue);

    // Update isteği - sadece ID'ler yeterli
    public sealed record PermissionAssignmentDto(
        Guid? UserId,
        Guid? RoleId,
        int PermissionValue);
}
