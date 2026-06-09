using Microsoft.EntityFrameworkCore;
using QrAssignment.Application.Features.Tenants.Queries.GetList;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Entity.App;
using QrAssignment.Persistance.Context;

namespace QrAssignment.Persistance.Repositories;

internal sealed class TenantRepository : GenericRepository<Tenant>, ITenantRepository
{
    private readonly AppDbContext _context;
    public TenantRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }





    public async Task<List<TenantListItemDto>> GetList(CancellationToken cancellationToken)
    {
        return await _context.Tenants
            .AsNoTracking()
            .Select(c => new TenantListItemDto
            {
                Id = c.Id, 
                Name = c.Name, 
            })
            .ToListAsync(cancellationToken);
    }
    public async Task<List<TenantItemDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Tenants
            .AsNoTracking()
            .Where(c => c.Id == id)
            .Select(c => new TenantItemDto
            {
                Id = c.Id, 
                Name = c.Name, 
            })
            .ToListAsync(cancellationToken);
    }

}

