using Microsoft.EntityFrameworkCore;
using QrAssignment.Application.DTOs.List;
using QrAssignment.Application.Features.Tenants.DTOs; 
using QrAssignment.Application.Features.Tenants.Queries.GetListExportExcel;
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
     
    public async Task<Paginate<TenantListItemDto>> GetDtoListAsync(PageRequestBaseDto request, CancellationToken cancellationToken)
    {
        IQueryable<Tenant> query = _context.Tenants 
            .AsNoTracking();
        
        return await GetPaginatedListAsync(
            query,
            request,
            CreateTenantListItemDto(),
            cancellationToken); ;
    }

    public async Task<Paginate<TenantListItemDto>> GetPassivedDtoListAsync(PageRequestBaseDto request, CancellationToken cancellationToken)
    {
        IQueryable<Tenant> query = _context.Tenants 
            .AsNoTracking();

        query = query.IgnoreQueryFilters(["SoftDeleteFilter"]).Where("IsPassived == true");

        return await GetPaginatedListAsync(
            query,
            request,
            CreateTenantListItemDto(),
            cancellationToken); ;
    }

    private static System.Linq.Expressions.Expression<Func<Tenant, TenantListItemDto>> CreateTenantListItemDto()
    {
        return t => new TenantListItemDto
        {
            Id = t.Id,
            Name = t.Name,
            RevNum = t.RevNum,
            CreatedUserFullName = t.CreatedByUser != null ? t.CreatedByUser.FullName : "",
            ModifiedUserFullName = t.ModifiedByUser != null ? t.ModifiedByUser.FullName : "",
            CreatedDateTime = t.CreatedDate,
            ModifiedDateTime = t.ModifiedDate

        };
    }

    public Task<List<TenantListItemExcelDto>> GetExportListAsync(GetTenantListExportExcelQuery request, CancellationToken cancellationToken)
    {
        IQueryable<Tenant> query = _context.Tenants.AsNoTracking();

        return GetFilteredListWithoutPaginationAsync(
            query,
            request, // ExportTenantsQuery artık PageRequestBaseDto'dan miras almalı
            t => new TenantListItemExcelDto { Name = t.Name },
            cancellationToken);
    }
    public async Task<TenantItemDto> GetDtoByIdAsync(Guid id, CancellationToken cancellationToken)
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


    public async Task<TenantItemDto> GetPassivedDtoByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Tenants.IgnoreQueryFilters(["SoftDeleteFilter"])
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

    public async Task BulkDelete(List<Guid> ids, CancellationToken cancellationToken)
    {
       await DeleteRange(ids, cancellationToken);
    }
}

