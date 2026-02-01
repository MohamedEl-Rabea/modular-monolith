using Microsoft.EntityFrameworkCore;

namespace WT.B2C.API.BuildingBlocks.Data.Queries;

public interface IQueryBuilderCreator<TContext> where TContext : DbContext
{
    IQueryBuilder Create(CancellationToken cancellationToken = default);
}
