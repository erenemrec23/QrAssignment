using MediatR;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.QrLocations.Queries.GetList
{
    public class GetListQrLocationQueryHandler : IRequestHandler<GetListQrLocationQuery, Result<List<GetListQrLocationResponse>>>
    {
        private readonly IQrLocationRepository _qrLocationRepository;

        public GetListQrLocationQueryHandler(IQrLocationRepository qrLocationRepository)
        {
            _qrLocationRepository = qrLocationRepository;
        }

        public async Task<Result<List<GetListQrLocationResponse>>> Handle(GetListQrLocationQuery request, CancellationToken cancellationToken)
        {
            var result = await _qrLocationRepository.GetCarsWithBrandAsync(cancellationToken);

            return Result.Success(result);
        }
    }
}
