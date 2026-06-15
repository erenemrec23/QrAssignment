using Microsoft.EntityFrameworkCore;
using QrAssignment.Application.DTOs.List;
using QrAssignment.Application.Extensions;
using QrAssignment.Application.Features.Tenants.Queries.GetList;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Entity.App;
using QrAssignment.Persistance.Context;
using System.Linq.Dynamic.Core;

namespace QrAssignment.Persistance.Repositories;

internal sealed class TenantRepository : GenericRepository<Tenant>, ITenantRepository
{
    private readonly AppDbContext _context;
    public TenantRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
     
    public async Task<Paginate<TenantListItemDto>> GetListAsync(GetListTenantQuery request, CancellationToken cancellationToken)
    { 
        IQueryable<Tenant> query = _context.Tenants.AsNoTracking();
         
        int totalItemCount = await query.CountAsync(cancellationToken);

        if (request.DynamicFilterAndSort != null)
        { 
            query = query.ToDynamic(request.DynamicFilterAndSort);
        }
         
        int totalFilteredItemCount = await query.CountAsync(cancellationToken);
         
        int size = request.PageSize;
        int index = request.PageIndex;

        var items = await query
            .Skip(index * size)
            .Take(size)
            .Select(t => new TenantListItemDto
            {
                Id = t.Id,
                Name = t.Name
            })
            .ToListAsync(cancellationToken);
         
        return new Paginate<TenantListItemDto>
        {
            Index = index,
            PageSize = size,
            TotalItemCount = totalItemCount,
            TotalFilteredItemCount = totalFilteredItemCount,
            Items = items
        };
    }
    public async Task<TenantItemDto> GetById(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Tenants
            .AsNoTracking()
            .Where(c => c.Id == id)
            .Select(c => new TenantItemDto
            {
                Id = c.Id, 
                Name = c.Name,
                RowVersion = c.RowVersion,
            })
            .SingleOrDefaultAsync(cancellationToken);
    }

}

