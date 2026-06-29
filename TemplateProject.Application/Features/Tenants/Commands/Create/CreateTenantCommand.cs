using QrAssignment.Application.Abstractions;
using QrAssignment.Application.Interfaces;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Tenants.Commands.Create
{
    public class CreateTenantCommand : ICommand<Result<Guid>>, ISecuredRequest
    {
        public string PageName => "Page_Tenants";
        public PagePermissions RequiredPermission => PagePermissions.Insert;
        public required string Name { get; set; }
    }
}
