using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using WT.B2C.API.BuildingBlocks.Types;

namespace WT.B2C.API.BuildingBlocks.Extensions;

public static class QueryablePagingSortingExtensions
{
    public static async Task<PagedResponse<T>> ToPagedResponseAsync<T>(this IQueryable<T> query,
        PagedAndSortedRequestDto request,
        CancellationToken cancellationToken = default)
    {
        query = query.ApplySorting(request);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .ApplyPaging(request)
            .ToListAsync(cancellationToken);

        var pageNumber = request.PageNumber < 1 ? 1 : request.PageNumber;
        var pageSize = request.PageSize < 1 ? 20 : request.PageSize;

        return new PagedResponse<T>
        {
            Items = items,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    /// <summary>
    /// Applies paging only.
    /// </summary>
    public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> query, PagedRequestDto request)
    {
        if (query is null) throw new ArgumentNullException(nameof(query));
        if (request is null) throw new ArgumentNullException(nameof(request));

        var pageNumber = request.PageNumber <= 0 ? 1 : request.PageNumber;
        var pageSize = request.PageSize <= 0 ? 20 : request.PageSize;

        var skip = (pageNumber - 1) * pageSize;

        return query.Skip(skip).Take(pageSize);
    }

    /// <summary>
    /// Applies dynamic sorting only (via System.Linq.Dynamic.Core).
    /// - If SortBy is null/empty => no sorting applied.
    /// - Supports nested paths like "Partner.Name".
    /// </summary>
    public static IQueryable<T> ApplySorting<T>(this IQueryable<T> query, PagedAndSortedRequestDto request)
    {
        if (query is null) throw new ArgumentNullException(nameof(query));
        if (request is null) throw new ArgumentNullException(nameof(request));

        if (string.IsNullOrWhiteSpace(request.SortBy))
            return query;

        var direction = request.SortDirection == SortDirection.Asc ? "ascending" : "descending";

        // Dynamic LINQ format: "PropertyName ascending|descending"
        // Example: "StartDate descending"
        var ordering = $"{request.SortBy} {direction}";

        return query.OrderBy(ordering);
    }
}