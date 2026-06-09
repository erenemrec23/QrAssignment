using MediatR;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Tenants.Queries.GetList
{
    public class GetListTenantQuery : IRequest<Result<List<TenantListItemDto>>>
    {
    }
}
