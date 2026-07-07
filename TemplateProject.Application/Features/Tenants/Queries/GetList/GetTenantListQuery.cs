using MediatR;
using QrAssignment.Application.DTOs.List;
using QrAssignment.Application.Interfaces;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Tenants.Queries.GetList
{
    public class GetTenantListQuery : PageRequestBaseDto, IRequest<Result<Paginate<TenantListItemDto>>>
    {

    }

    
}
