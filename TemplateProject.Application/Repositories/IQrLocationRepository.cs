using QrAssignment.Application.DTOs.List;
using QrAssignment.Application.Features.QrLocations.Queries.GetById;
using QrAssignment.Application.Features.QrLocations.Queries.GetList;
using QrAssignment.Application.Interfaces;
using QrAssignment.Domain.Entity;

namespace QrAssignment.Application.Repositories
{
    public interface IQrLocationRepository : IGenericRepository<QrLocation>
    {
        Task<Paginate<QrLocationListItemDto>> GetList(PageRequestBaseDto request, CancellationToken cancellationToken);
        Task<List<QrLocationItemGetByIdDto>> GetById(Guid id, CancellationToken cancellationToken);
    }
}
