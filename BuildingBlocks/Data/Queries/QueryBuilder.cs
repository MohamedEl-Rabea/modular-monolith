using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using WT.B2C.API.BuildingBlocks.Cache;

namespace WT.B2C.API.BuildingBlocks.Data.Queries;

public class QueryBuilder<TContext> : IQueryBuilder
    where TContext : DbContext
{
    private readonly TContext _context;
    private readonly CancellationToken _ct;

    public QueryBuilder(TContext context, CancellationToken ct = default)
    {
        _context = context;
        _ct = ct;
        _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    public IQueryable<TEntity> Query<TEntity>() where TEntity : class
    {
        return _context.Set<TEntity>();
    }

    public Task<IReadOnlyList<TEntity>> CachedQueryAsync<TEntity>() where TEntity : class
    {
        var cacheManager = _context.GetService<ICacheManager<TEntity>>();
        return cacheManager.GetAllAsync(_ct);
    }

    public void Dispose()
    {
        _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }
}