namespace WT.B2C.API.BuildingBlocks.Data.Queries;

public interface IQueryBuilder : IDisposable
{
    IQueryable<TEntity> Query<TEntity>() where TEntity : class;
    Task<IReadOnlyList<TEntity>> CachedQueryAsync<TEntity>() where TEntity : class;
}