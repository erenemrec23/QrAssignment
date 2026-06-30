 
using Microsoft.EntityFrameworkCore;
using QrAssignment.Application.DTOs.List;
using QrAssignment.Application.Features.QrLocations.Queries.GetById;
using QrAssignment.Application.Features.QrLocations.Queries.GetList;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Entity; 
using QrAssignment.Persistance.Context;

namespace QrAssignment.Persistance.Repositories;

internal sealed class QrLocationRepository : GenericRepository<QrLocation>, IQrLocationRepository
{
    public QrLocationRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
    private readonly AppDbContext _context;
     
    public async Task<Paginate<QrLocationListItemDto>> GetList(PageRequestBaseDto request, CancellationToken cancellationToken)
    {

        IQueryable<QrLocation> query = _context.QrLocations.AsNoTracking();
        return await GetPaginatedListAsync(
            query,
            request, c => new QrLocationListItemDto
            { 
                Id = c.Id,
                EndDate = c.EndDate,
                LocationName = c.LocationName,
                Name = c.Name,
                ParentLocationId = c.ParentLocationId,
                ParentLocationName = c.ParentLocation != null ? c.ParentLocation.LocationName : null,
                StartDate = c.StartDate,
                RowVersion = c.RowVersion
            });
    }
    public async Task<List<QrLocationItemGetByIdDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        return await _context.QrLocations
            .AsNoTracking()
            .Where(c => c.Id == id)
            .Select(c => new QrLocationItemGetByIdDto
            {
                Id = c.Id,
                EndDate = c.EndDate,
                LocationName = c.LocationName,
                Name = c.Name,
                ParentLocationId = c.ParentLocationId,
                ParentLocationName = c.ParentLocation != null ? c.ParentLocation.LocationName : null,
                StartDate = c.StartDate,
                RowVersion = c.RowVersion
            })
            .ToListAsync(cancellationToken);
    }

}

