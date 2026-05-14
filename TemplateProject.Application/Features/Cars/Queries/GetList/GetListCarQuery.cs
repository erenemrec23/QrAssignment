using MediatR;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Cars.Queries.GetList
{
    public class GetListCarQuery : IRequest<Result<List<GetListCarResponse>>>
    {
    }


}
