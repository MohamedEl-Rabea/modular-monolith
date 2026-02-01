namespace WT.B2C.API.BuildingBlocks.Types;

public record PagedResponse<TItem>
{
    public required IReadOnlyList<TItem> Items { get; init; }

    /// <summary>
    /// 1-based page index.
    /// </summary>
    public int PageNumber { get; init; }

    /// <summary>
    /// Number of items per page.
    /// </summary>
    public int PageSize { get; init; }

    /// <summary>
    /// Total number of items (without paging).
    /// </summary>
    public long TotalCount { get; init; }

    /// <summary>
    /// Total number of pages.
    /// </summary>
    public int TotalPages =>
        PageSize <= 0 ? 0 : (int)Math.Ceiling(TotalCount / (double)PageSize);

    public bool HasNextPage => PageNumber < TotalPages;
    public bool HasPreviousPage => PageNumber > 1;
}