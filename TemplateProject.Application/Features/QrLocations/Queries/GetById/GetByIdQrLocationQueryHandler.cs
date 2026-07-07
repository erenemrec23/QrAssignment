using MediatR;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.QrLocations.Queries.GetById
{
    public class GetByIdQrLocationQueryHandler : IRequestHandler<GetQrLocationByIdQuery, Result<List<QrLocationItemGetByIdDto>>>
    {
        private readonly IQrLocationRepository _qrLocationRepository;

        public GetByIdQrLocationQueryHandler(IQrLocationRepository qrLocationRepository)
        {
            _qrLocationRepository = qrLocationRepository;
        }

        public async Task<Result<List<QrLocationItemGetByIdDto>>> Handle(GetQrLocationByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _qrLocationRepository.GetById(request.Id, cancellationToken);

            return Result.Success(result);
        }
    }
}
