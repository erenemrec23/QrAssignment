using Microsoft.EntityFrameworkCore;
using QrAssignment.Application.DTOs.List;
using QrAssignment.Application.Extensions;
using QrAssignment.Application.Interfaces;
using QrAssignment.Domain.Abstractions;
using QrAssignment.Persistance.Context;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace QrAssignment.Persistance.Repositories
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

        public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }

        public async Task<T?> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public async Task<List<T>> GetAllAsync(bool tracking = true, CancellationToken cancellationToken = default)
        {
            var query = _dbSet.AsQueryable();
            if (!tracking) query = query.AsNoTracking();

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<List<T>> GetWhereAsync(Expression<Func<T, bool>> predicate, bool tracking = true, CancellationToken cancellationToken = default)
        {
            var query = _dbSet.Where(predicate);
            if (!tracking) query = query.AsNoTracking();

            return await query.ToListAsync(cancellationToken);
        }

        public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddAsync(entity, cancellationToken);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddRangeAsync(entities, cancellationToken);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);

            if (entity.RowVersion != null)
            {
                var entry = _context.Entry(entity);

                var rowVersionProperty = entry.Properties.FirstOrDefault(p => p.Metadata.Name == "RowVersion");

                if (rowVersionProperty != null)
                {
                    rowVersionProperty.OriginalValue = entity.RowVersion;
                }
            }
        }
        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var entity = await GetByIdAsync(id, cancellationToken);
            if (entity is not null)
            {
                Delete(entity);
            }
        }

        protected async Task<Paginate<TDto>> GetPaginatedListAsync<TDto>(
      IQueryable<T> query,
      PageRequestBaseDto request,
      Expression<Func<T, TDto>> projection, // Entity'yi DTO'ya çevirecek kural
      CancellationToken cancellationToken = default)
        {
                         
            // 1. Filtresiz toplam kayıt sayısı
            int totalItemCount = await query.CountAsync(cancellationToken);

            // 2. Filtreleri ve Global Arama'yı uygula
            query = ApplyFilters(query, request.DynamicFilterAndSort, request.GlobalSearch);

            // 3. Filtreli toplam kayıt sayısı
            int totalFilteredItemCount = await query.CountAsync(cancellationToken);

            // 4. Sayfalama ve DTO'ya dönüştürme işlemi
            var items = await query
                .Skip(request.PageIndex * request.PageSize)
                .Take(request.PageSize)
                .Select(projection) // Dışarıdan gelen DTO haritalama kuralını uyguluyoruz
                .ToListAsync(cancellationToken);

            return new Paginate<TDto>
            {
                Index = request.PageIndex,
                PageSize = request.PageSize,
                TotalItemCount = totalItemCount,
                TotalFilteredItemCount = totalFilteredItemCount,
                Items = items
            };
        }

        /// <summary>
        /// Jenerik IQueryable filtreleme yardımcı metodu
        /// </summary>
        private IQueryable<T> ApplyFilters(IQueryable<T> query, DynamicQueryDto? dynamicFilter, GlobalSearchDto? globalSearch)
        {
            if (globalSearch != null && globalSearch.Fields.Any() && !string.IsNullOrWhiteSpace(globalSearch.Value))
            {
                string searchClause = string.Join(" || ", globalSearch.Fields.Select(field => $"{field}.Contains(@0)"));
                query = query.Where(searchClause, globalSearch.Value);
            }

            if (dynamicFilter != null)
            {
                query = query.ToDynamic(dynamicFilter);
            }

            return query;
        }

       

        // BaseRepository.cs içine eklenecek:

        protected async Task<List<TDto>> GetFilteredListWithoutPaginationAsync<TDto>(
            IQueryable<T> query,
            PageRequestBaseDto request,
            Expression<Func<T, TDto>> projection,
            CancellationToken cancellationToken = default)
        { 
            query = ApplyFilters(query, request.DynamicFilterAndSort, request.GlobalSearch);

            return await query
                .Select(projection)
                .ToListAsync(cancellationToken);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public async Task DeleteRange(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
        {
            var entities = await _dbSet
                .Where(e => ids.Contains(e.Id))
                .ToListAsync(cancellationToken);

            if (entities.Any())
            {
                DeleteRange(entities);
            }
        }
    }
}