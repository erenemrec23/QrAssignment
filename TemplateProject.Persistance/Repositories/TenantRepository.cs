using Microsoft.EntityFrameworkCore;
using QrAssignment.Application.DTOs.List;
using QrAssignment.Application.Features.Tenants.Commands.Excel.BulkCreate;
using QrAssignment.Application.Features.Tenants.DTOs;
using QrAssignment.Application.Features.Tenants.Queries.GetListExportExcel;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Entity.App;
using QrAssignment.Persistance.Context;
using QrAssignment.Persistance.Repositories.Base;

internal sealed class TenantRepository : GenericRepository<Tenant>, ITenantRepository
{
    private readonly AppDbContext _context;
    public TenantRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
     
    private DbSet<Tenant> Tenants => _context.Tenants;
    private IQueryable<Tenant> TenantsNoTracking => Tenants.AsNoTracking();
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

    public async Task<Paginate<TenantListItemDto>> GetDtoListAsync(PageRequestBaseDto request, CancellationToken cancellationToken)
    {
        return await GetPaginatedListAsync(
            TenantsNoTracking,
            request,
            CreateTenantListItemDto(),
            cancellationToken);
    }

    public async Task<Paginate<TenantListItemDto>> GetPassivedDtoListAsync(PageRequestBaseDto request, CancellationToken cancellationToken)
    {
        IQueryable<Tenant> query = TenantsNoTracking
            .IgnoreQueryFilters(["SoftDeleteFilter"])
            .Where(w=>w.IsPassived == true);

        return await GetPaginatedListAsync(
            query,
            request,
            CreateTenantListItemDto(),
            cancellationToken);
    }


    public Task<List<TenantListItemExcelDto>> GetExportListAsync(PageRequestBaseDto request, CancellationToken cancellationToken)
    {
        return GetFilteredListWithoutPaginationAsync(
            TenantsNoTracking,
            request,
            t => new TenantListItemExcelDto { Name = t.Name, Code = t.RevNum.ToString() },
            cancellationToken);
    }

    public async Task<TenantItemDto> GetDtoByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await TenantsNoTracking
            .Where(c => c.Id == id)
            .Select(c => ConvertToTenantItemDto(c))
            .SingleOrDefaultAsync(cancellationToken);
    }

    private static TenantItemDto ConvertToTenantItemDto(Tenant c)
    {
        return new TenantItemDto
        {
            Id = c.Id,
            Name = c.Name,
            RowVersion = c.RowVersion,
        };
    }

    public async Task<TenantItemDto> GetPassivedDtoByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await TenantsNoTracking
            .IgnoreQueryFilters(["SoftDeleteFilter"])
            .Where(c => c.Id == id)
            .Select(c => ConvertToTenantItemDto(c))
            .SingleOrDefaultAsync(cancellationToken);
    } 
    public async Task BulkDelete(List<Guid> ids, CancellationToken cancellationToken)
    {
        await DeleteRange(ids, cancellationToken);
    }
     

    public async Task<List<Tenant>> GetByRevNumsAsync(List<long> revnums, CancellationToken cancellationToken)
    {
        if (revnums == null || !revnums.Any())
            return new List<Tenant>();

        return await TenantsNoTracking
            .Where(u => revnums.Contains(u.RevNum))
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Tenant>> GetByNamesAsync(List<string> names, CancellationToken cancellationToken)
    {
        if (names == null || !names.Any())
            return new List<Tenant>();

        return await TenantsNoTracking
            .Where(u => names.Contains(u.Name))
            .ToListAsync(cancellationToken);
    }
}