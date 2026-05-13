using QrAssignment.Application.Features.Cars.Queries.GetList;
using QrAssignment.Application.Interfaces;
using QrAssignment.Domain.Entity;

namespace QrAssignment.Application.Repositories
{
    public interface ICarRepository : IGenericRepository<Car>
    { 
        Task<List<GetListCarResponse>> GetCarsWithBrandAsync(CancellationToken cancellationToken);
    }
}
