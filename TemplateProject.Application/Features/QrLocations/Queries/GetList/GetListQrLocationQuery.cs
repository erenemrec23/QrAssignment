using MediatR;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.QrLocations.Queries.GetList
{
    public class GetListQrLocationQuery : IRequest<Result<List<QrLocationListItemDto>>>
    {
    }


}
