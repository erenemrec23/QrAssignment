using MediatR;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Cars.Queries.GetList
{
    public class GetListCarQueryHandler : IRequestHandler<GetListCarQuery, Result<List<GetListCarResponse>>>
    { 
        private readonly ICarRepository _carRepository; 

        public GetListCarQueryHandler(ICarRepository carRepository)
        {
            _carRepository = carRepository;
        }

        public async Task<Result<List<GetListCarResponse>>> Handle(GetListCarQuery request, CancellationToken cancellationToken)
        {
            var result = await _carRepository.GetCarsWithBrandAsync(cancellationToken);
            
            return Result.Success(result);
        }
    }
}
