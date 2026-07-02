using MediatR;
using QrAssignment.Application.Interfaces;
using QrAssignment.Domain.Shared; 

namespace QrAssignment.Application.Features.AppRole.Commands.Update
{
    // Command
    public sealed record UpdateAppRoleCommand(string Id, string Name) : IRequest<Result>, ISecuredRequest
    {
        public string PageName => "Page_AppRoles";
        public PagePermissions RequiredPermission => PagePermissions.Update;
    };
}