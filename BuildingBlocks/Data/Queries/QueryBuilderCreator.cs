using Microsoft.EntityFrameworkCore;

namespace WT.B2C.API.BuildingBlocks.Data.Queries;

public class QueryBuilderCreator<TContext>(TContext appDbContext) : IQueryBuilderCreator<TContext>
    where TContext : DbContext
{
    public IQueryBuilder Create(CancellationToken ct = default)
    {
        return new QueryBuilder<TContext>(appDbContext);
    }
}