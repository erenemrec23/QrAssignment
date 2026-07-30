using Microsoft.EntityFrameworkCore;
using QrAssignment.Application.DTOs.List;
using QrAssignment.Application.Features.Tenants.DTOs;
using QrAssignment.Application.Features.Tenants.Queries.DTOs;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Entity.App;
using QrAssignment.Persistance.Context;
using QrAssignment.Persistance.Repositories.Base;
using System.Linq.Expressions;

internal sealed class TenantRepository : GenericRepository<Tenant>, ITenantRepository
{
    private readonly AppDbContext _context;
    public TenantRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    // --- Okuma kaynakları ---
    private IQueryable<Tenant> TenantsNoTracking => _context.Tenants.AsNoTracking();

    private IQueryable<Tenant> PassivedTenants =>
        TenantsNoTracking.IgnoreQueryFilters(["SoftDeleteFilter"]).Where(t => t.IsPassived);

    // --- Projeksiyonlar ---
    private static Expression<Func<Tenant, TenantListItemDto>> ListProjection =>
        t => new TenantListItemDto
        {
            Id = t.Id,
            Name = t.Name,
            RevNum = t.RevNum,
            CreatedUserFullName = t.CreatedByUser != null ? t.CreatedByUser.FullName : "",
            ModifiedUserFullName = t.ModifiedByUser != null ? t.ModifiedByUser.FullName : "",
            CreatedDateTime = t.CreatedDate,
            ModifiedDateTime = t.ModifiedDate
        };

    private static Expression<Func<Tenant, TenantItemDto>> ItemProjection =>
        t => new TenantItemDto
        {
            Id = t.Id,
            Name = t.Name,
            RowVersion = t.RowVersion
        };

    private static Expression<Func<Tenant, TenantListItemExcelDto>> ExcelProjection =>
        t => new TenantListItemExcelDto
        {
            Name = t.Name,
            Code = t.RevNum.ToString()
        };

    // --- Liste / export ---
    public Task<Paginate<TenantListItemDto>> GetDtoListAsync(PageRequestBaseDto request, CancellationToken cancellationToken)
        => GetPaginatedListAsync(TenantsNoTracking, request, ListProjection, cancellationToken);

    public Task<Paginate<TenantListItemDto>> GetPassivedDtoListAsync(PageRequestBaseDto request, CancellationToken cancellationToken)
        => GetPaginatedListAsync(PassivedTenants, request, ListProjection, cancellationToken);

    public Task<List<TenantListItemExcelDto>> GetExportListAsync(PageRequestBaseDto request, CancellationToken cancellationToken)
        => GetFilteredListWithoutPaginationAsync(TenantsNoTracking, request, ExcelProjection, cancellationToken);

    // --- Tekil ---
    public Task<TenantItemDto?> GetDtoByIdAsync(Guid id, CancellationToken cancellationToken)
        => TenantsNoTracking
            .Where(t => t.Id == id)
            .Select(ItemProjection)
            .SingleOrDefaultAsync(cancellationToken);

    public Task<TenantItemDto?> GetPassivedDtoByIdAsync(Guid id, CancellationToken cancellationToken)
        => PassivedTenants
            .Where(t => t.Id == id)
            .Select(ItemProjection)
            .SingleOrDefaultAsync(cancellationToken);


    public Task<Tenant?> GetPassivedByIdAsync(Guid id, CancellationToken cancellationToken)
        => PassivedTenants
            .Where(t => t.Id == id)
            .SingleOrDefaultAsync(cancellationToken);

    // --- Silme ---
    public Task BulkDelete(List<Guid> ids, CancellationToken cancellationToken)
        => DeleteRange(ids, cancellationToken);

    // --- Değer listesiyle (IN) sorgular ---
    public Task<List<Tenant>> GetByRevNumsAsync(List<long> revnums, CancellationToken cancellationToken)
        => GetByValuesAsync(t => t.RevNum, revnums, cancellationToken: cancellationToken);

    public Task<List<Tenant>> GetByNamesAsync(List<string> names, CancellationToken cancellationToken)
        => GetByValuesAsync(t => t.Name, names, cancellationToken: cancellationToken);


    public Task BulkActiveByIds(List<Guid> ids, CancellationToken cancellationToken)
        => BulkSetActiveByIdsAsync( ids, cancellationToken: cancellationToken);
    public Task SetActiveById(Guid id, CancellationToken cancellationToken)
        => SetActiveAsync(id, cancellationToken: cancellationToken);


    public Task SetPassiveById(Guid id, CancellationToken cancellationToken)
        => SetPassiveAsync(id, cancellationToken: cancellationToken);


    public Task BulkSetPassiveByIds(List<Guid> ids, CancellationToken cancellationToken)
        => BulkSetPassiveByIdsAsync(ids, cancellationToken: cancellationToken);
}