using QrAssignment.Domain.Abstractions;
using System.Linq.Expressions;

namespace QrAssignment.Application.Interfaces;

public interface IGenericRepository<T> where T : class, IBaseEntity
{
    // READ
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<T?> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    Task<List<T>> GetAllAsync(bool tracking = true, CancellationToken cancellationToken = default);
    Task<List<T>> GetWhereAsync(Expression<Func<T, bool>> predicate, bool tracking = true, CancellationToken cancellationToken = default);

    // WRITE
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
    void Update(T entity);
    void Delete(T entity);
    Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default);
}