using Microsoft.EntityFrameworkCore;
using QrAssignment.Application.DTOs.List;
using QrAssignment.Domain.Abstractions;   // IBaseEntity
using QrAssignment.Persistance.Context;
using System.Linq.Expressions;

namespace QrAssignment.Persistance.Repositories;

// NOT: IsPassived IBaseEntity'de değilse, soft-delete arayüzünü de constraint'e ekle.
internal abstract class GenericAppRepository<TEntity> where TEntity : class, IBaseEntity
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<TEntity> _set;

    protected GenericAppRepository(AppDbContext context)
    {
        _context = context;
        _set = context.Set<TEntity>();
    }

    // Okuma kaynağı. RoleManager.Roles kullanmak istersen türeyen sınıfta override et.
    protected virtual IQueryable<TEntity> Query => _set.AsNoTracking();

    protected Task<Paginate<TDto>> PaginateAsync<TDto>(
        Expression<Func<TEntity, TDto>> projection, PageRequestBaseDto request, CancellationToken ct)
        => Query.ToPaginateAsync(request, projection, ct);

    protected Task<Paginate<TDto>> PaginatePassivedAsync<TDto>(
        Expression<Func<TEntity, TDto>> projection, PageRequestBaseDto request, CancellationToken ct)
        => Query.IgnoreQueryFilters(["SoftDeleteFilter"])
                .Where(e => e.IsPassived)
                .ToPaginateAsync(request, projection, ct);

    protected Task<List<TDto>> ListAsync<TDto>(
        Expression<Func<TEntity, TDto>> projection, PageRequestBaseDto request, CancellationToken ct)
        => Query.ToFilteredListAsync(request, projection, ct);

    protected Task<TDto?> SingleDtoByIdAsync<TDto>(
        Guid id, Expression<Func<TEntity, TDto>> projection, CancellationToken ct) where TDto : class
        => Query.Where(e => e.Id == id).Select(projection).SingleOrDefaultAsync(ct);

    protected Task<TDto?> SinglePassivedDtoByIdAsync<TDto>(
        Guid id, Expression<Func<TEntity, TDto>> projection, CancellationToken ct) where TDto : class
        => Query.IgnoreQueryFilters(["SoftDeleteFilter"])
                .Where(e => e.Id == id).Select(projection).SingleOrDefaultAsync(ct);

    protected async Task RemoveByIdsAsync(List<Guid> ids, CancellationToken ct)
    {
        var entities = await _set.Where(e => ids.Contains(e.Id)).ToListAsync(ct);
        if (entities.Count > 0)
            _set.RemoveRange(entities); // Soft-delete interceptor + UnitOfWork pipeline commit eder
    }
}