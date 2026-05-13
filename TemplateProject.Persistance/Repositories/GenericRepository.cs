using Microsoft.EntityFrameworkCore;
using QrAssignment.Application.Interfaces;
using QrAssignment.Domain.Abstractions;
using QrAssignment.Persistance.Context;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

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
    }
}
