using Microsoft.EntityFrameworkCore;
using QrAssignment.Application.DTOs.List;
using QrAssignment.Application.Interfaces;
using QrAssignment.Domain.Abstractions;
using QrAssignment.Persistance.Context;
using System.Linq.Expressions;

namespace QrAssignment.Persistance.Repositories.Base
{
    internal class GenericRepository<T> : IGenericRepository<T> where T : class, IBaseEntity
    {
        protected readonly DbContext _context; // Türeyen sınıflar erişebilsin diye protected
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => _dbSet.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

        public Task<T?> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
            => _dbSet.FirstOrDefaultAsync(predicate, cancellationToken);

        public Task<List<T>> GetAllAsync(bool tracking = true, CancellationToken cancellationToken = default)
            => Query(tracking).ToListAsync(cancellationToken);

        public Task<List<T>> GetWhereAsync(Expression<Func<T, bool>> predicate, bool tracking = true, CancellationToken cancellationToken = default)
            => Query(tracking).Where(predicate).ToListAsync(cancellationToken);

        public Task AddAsync(T entity, CancellationToken cancellationToken = default)
            => _dbSet.AddAsync(entity, cancellationToken).AsTask();

        public Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
            => _dbSet.AddRangeAsync(entities, cancellationToken);

        public void Update(T entity)
        {
            var entry = _dbSet.Update(entity);

            if (entity.RowVersion is not null)
                entry.Property(nameof(IBaseEntity.RowVersion)).OriginalValue = entity.RowVersion;
        }

        public void Delete(T entity) => _dbSet.Remove(entity);

        public void DeleteRange(IEnumerable<T> entities) => _dbSet.RemoveRange(entities);

        public async Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var entity = await GetByIdAsync(id, cancellationToken);
            if (entity is not null)
                Delete(entity);
        }

        public async Task DeleteRange(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
        {
            var entities = await _dbSet
                .Where(e => ids.Contains(e.Id))
                .ToListAsync(cancellationToken);

            if (entities.Count > 0)
                DeleteRange(entities);
        }

        // --- Sayfalama / filtreleme: ortak extension'a delege ediliyor ---

        protected Task<Paginate<TDto>> GetPaginatedListAsync<TDto>(
            IQueryable<T> query,
            PageRequestBaseDto request,
            Expression<Func<T, TDto>> projection,
            CancellationToken cancellationToken = default)
            => query.ToPaginateAsync(request, projection, cancellationToken);
         

        // --- Türeyen sınıflara okuma kaynağı ---
        protected Task<List<TDto>> GetFilteredListWithoutPaginationAsync<TDto>(
           IQueryable<T> query,
           PageRequestBaseDto request,
           Expression<Func<T, TDto>> projection,
           CancellationToken cancellationToken = default)
           => query.ToFilteredListAsync(request, projection, cancellationToken);

        // YENİ: values.Contains(selector(e)) → SQL IN (...). Null/boş listede DB'ye gitmez.
        protected Task<List<T>> GetByValuesAsync<TValue>(
        Expression<Func<T, TValue>> selector,
        IReadOnlyCollection<TValue> values,
        bool tracking = false,
        CancellationToken cancellationToken = default)
        => Query(tracking).ToListByValuesAsync(selector, values, cancellationToken);

        // --- Türeyen sınıflara okuma kaynağı ---

        private IQueryable<T> Query(bool tracking)
            => tracking ? _dbSet : _dbSet.AsNoTracking();
        private static readonly string[] SoftDeleteFilterOnly = { "SoftDeleteFilter" };

        public async Task SetActiveAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var entity = await _dbSet
                .IgnoreQueryFilters(SoftDeleteFilterOnly)
                .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

            if (entity is ISoftDelete softDeleteEntity)
                softDeleteEntity.IsPassived = false;
        }

        public async Task BulkSetActiveByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
        {
            var idList = ids as ICollection<Guid> ?? ids.ToList();
            if (idList.Count == 0)
                return;

            var entities = await _dbSet
                .IgnoreQueryFilters(SoftDeleteFilterOnly)
                .Where(e => idList.Contains(e.Id))
                .ToListAsync(cancellationToken);

            foreach (var entity in entities)
            {
                if (entity is ISoftDelete softDeleteEntity)
                    softDeleteEntity.IsPassived = false;
            }
        }

    }
}