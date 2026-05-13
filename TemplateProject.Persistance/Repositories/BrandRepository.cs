
using Microsoft.EntityFrameworkCore;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Entity;
using QrAssignment.Persistance.Context;

namespace QrAssignment.Persistance.Repositories;

internal sealed class BrandRepository : GenericRepository<Brand>, IBrandRepository
{
    public BrandRepository(AppDbContext context) : base(context)
    {
    }
}
