using MediatR;
using QrAssignment.Application.DTOs.List;
using QrAssignment.Application.Features.Tenants.Queries.GetList;
using QrAssignment.Application.Interfaces;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.QrLocations.Queries.GetList
{ 
    public class GetListQrLocationQuery : PageRequestBaseDto, IRequest<Result<Paginate<QrLocationListItemDto>>>, ISecuredRequest
    {

        public string PageName => "Page_QrLocations";
        public PagePermissions RequiredPermission => PagePermissions.View;
    }


}
