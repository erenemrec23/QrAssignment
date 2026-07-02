using MediatR;
using QrAssignment.Application.Interfaces;
using QrAssignment.Domain.Entity.App;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.AppRole.Commands.Delete
{
    // Command
    public sealed record DeleteAppRoleCommand(string Id) : IRequest<Result>, ISecuredRequest
    {
        public string PageName => "Page_AppRoles";
        public PagePermissions RequiredPermission => PagePermissions.Delete;
    };
}