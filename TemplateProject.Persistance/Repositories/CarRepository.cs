
using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TemplateProject.Application.Features.Cars.Queries.GetList;
using TemplateProject.Application.Interfaces;
using TemplateProject.Application.Repositories;
using TemplateProject.Domain.Entity;
using TemplateProject.Domain.Entity.App;
using TemplateProject.Persistance.Repositories;
using TemplateProject.Persistence.Context;

namespace TemplateProject.Persistence.Repositories;

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

 
