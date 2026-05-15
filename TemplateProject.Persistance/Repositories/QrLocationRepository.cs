using Microsoft.EntityFrameworkCore;
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

     



    public async Task<List<GetListQrLocationResponse>> GetCarsWithBrandAsync(CancellationToken cancellationToken)
    {
        // Sorgu mantığı artık tamamen repository içinde kapsüllendi (encapsulated)
        return await _context.QrLocations
            .AsNoTracking()
            .Select(c => new GetListQrLocationResponse
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

