namespace QrAssignment.Application.Features.Roles.Commands.DTOs
{
    public sealed record RolePagePermissionDto(string? PageName, string? GroupKey, int PermissionValue);
}