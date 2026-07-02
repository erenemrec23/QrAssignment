using MediatR;
using QrAssignment.Application.DTOs.List;
using QrAssignment.Application.Interfaces;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Tenants.Queries.GetList
{
    public class GetListTenantQuery : PageRequestBaseDto, IRequest<Result<Paginate<TenantListItemDto>>>, ISecuredRequest
    {

        public string PageName => "Page_Tenants";
        public PagePermissions RequiredPermission => PagePermissions.View;
    }

    
}
