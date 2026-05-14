using QrAssignment.Application.Features.QrLocations.Queries.GetList;
using QrAssignment.Application.Interfaces;
using QrAssignment.Domain.Entity;

namespace QrAssignment.Application.Repositories
{
    public interface IQrLocationRepository : IGenericRepository<QrLocation>
    {
        Task<List<GetListQrLocationResponse>> GetCarsWithBrandAsync(CancellationToken cancellationToken);
    }
}
