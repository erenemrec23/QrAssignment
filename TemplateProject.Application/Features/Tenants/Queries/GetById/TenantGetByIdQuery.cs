using MediatR;
using QrAssignment.Application.Abstractions;
using QrAssignment.Application.Interfaces;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Tenants.Queries.GetById
{
    public class TenantGetByIdQuery : ICommand<Result<TenantItemDto>>, ISecuredRequest
    {
        public string PageName => "Page_Tenants";
        public PagePermissions RequiredPermission => PagePermissions.View;
        public Guid Id { get; set; }

        public TenantGetByIdQuery(Guid id)
        {
            Id = id;
        }
    } 
}
