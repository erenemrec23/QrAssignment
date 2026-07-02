using MediatR;
using QrAssignment.Application.Abstractions;
using QrAssignment.Application.Interfaces;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Tenants.Commands.Delete
{
    public class DeleteTenantCommand : ICommand<Result<DeleteTenantResponse>>, ISecuredRequest
    {

        public string PageName => "Page_Tenants";
        public PagePermissions RequiredPermission => PagePermissions.Delete;
        public Guid? Id { get; set; }
    }
}