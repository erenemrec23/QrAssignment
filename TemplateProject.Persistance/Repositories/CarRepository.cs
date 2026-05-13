
using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QrAssignment.Application.Features.Cars.Queries.GetList;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Entity;
using QrAssignment.Application.Interfaces;
using QrAssignment.Domain.Entity.App;
using QrAssignment.Persistance.Context;

namespace QrAssignment.Persistance.Repositories;

internal sealed class CarRepository : GenericRepository<Car>, ICarRepository
{
    private readonly AppDbContext _context;

    public CarRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

   

    public async Task<List<GetListCarResponse>> GetCarsWithBrandAsync(CancellationToken cancellationToken)
    {
        // Sorgu mantığı artık tamamen repository içinde kapsüllendi (encapsulated)
        return await _context.Cars
            .AsNoTracking()
            .Select(c => new GetListCarResponse
            {
                CarId = c.Id,
                BrandId = c.BrandId,
                Model = c.Model,
                Year = c.Year,
                BrandName = c.Brand.Name,
                BrandVersion = c.Brand.RowVersion,
                CarVersion = c.RowVersion
            })
            .ToListAsync(cancellationToken);
    }
}

 
