using MediatR;
using QrAssignment.Application.Interfaces;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.AppRole.Commands.Create
{
    // Command
    public sealed record CreateAppRoleCommand(string Name) : IRequest<Result>, ISecuredRequest
    {
        public string PageName => "Page_AppRoles";
        public PagePermissions RequiredPermission => PagePermissions.View; 
    };
}