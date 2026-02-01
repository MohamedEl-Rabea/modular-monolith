using System.Linq.Expressions;
using WT.B2C.API.BuildingBlocks.Domain;

namespace WT.B2C.API.BuildingBlocks.Data.Repositories;

public interface IGenericRepository<T> : IGenericRepository<T, long> where T : Entity<long>
{
}

public interface IGenericRepository<T, TKey> where T : Entity<TKey>
{
    void Add(T entity);
    Task<T?> GetById(TKey id);
    Task<T?> Get(Expression<Func<T, bool>> predicate);
    Task<T> GetById(TKey id, string[] includes);
    Task<bool> Exists(TKey id);
    Task<bool> Remove(TKey id);
    Task<bool> RemoveAll(Expression<Func<T, bool>> predicate);
    void Remove(T entity);
    Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
}