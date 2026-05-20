using MediatR;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.QrLocations.Queries.GetList
{
    public class GetListQrLocationQueryHandler : IRequestHandler<GetListQrLocationQuery, Result<List<QrLocationListItemDto>>>
    {
        private readonly IQrLocationRepository _qrLocationRepository;

        public GetListQrLocationQueryHandler(IQrLocationRepository qrLocationRepository)
        {
            _qrLocationRepository = qrLocationRepository;
        }

        public async Task<Result<List<QrLocationListItemDto>>> Handle(GetListQrLocationQuery request, CancellationToken cancellationToken)
        {
            var result = await _qrLocationRepository.GetList(cancellationToken);

            return Result.Success(result);
        }
    }
}
