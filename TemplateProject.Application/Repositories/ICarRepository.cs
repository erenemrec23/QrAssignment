using TemplateProject.Application.Features.Cars.Queries.GetList;
using TemplateProject.Application.Interfaces;
using TemplateProject.Domain.Entity;

namespace TemplateProject.Application.Repositories
{
    public interface ICarRepository : IGenericRepository<Car>
    { 
        Task<List<GetListCarResponse>> GetCarsWithBrandAsync(CancellationToken cancellationToken);
    }
}
