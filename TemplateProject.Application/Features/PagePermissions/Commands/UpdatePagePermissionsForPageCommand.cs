// Application/Features/Permission/Commands/UpdatePagePermissionsForPage/UpdatePagePermissionsForPageCommand.cs
using MediatR;
using QrAssignment.Application.Abstractions;
using QrAssignment.Application.Features.PagePermissions.DTOs;
using QrAssignment.Application.Security;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Permission.Commands.UpdatePagePermissionsForPage
{
    public sealed record UpdatePagePermissionsForPageCommand(
        string PageKey,
        List<PermissionAssignmentDto> Assignments)
        : IPageScopedRequest, IRequest<Result>;


}