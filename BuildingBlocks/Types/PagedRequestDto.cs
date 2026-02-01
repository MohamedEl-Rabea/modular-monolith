namespace WT.B2C.API.BuildingBlocks.Types;

public record PagedRequestDto
{
    /// <summary>
    /// 1-based page index.
    /// </summary>
    public int PageNumber { get; init; } = 1;

    /// <summary>
    /// Number of items per page.
    /// </summary>
    public int PageSize { get; init; } = 20;
}

public record PagedAndSortedRequestDto : PagedRequestDto
{
    /// <summary>
    /// Field name to sort by (e.g. "startDate", "name").
    /// </summary>
    public string SortBy { get; init; } = "id";

    /// <summary>
    /// Sort direction.
    /// </summary>
    public SortDirection SortDirection { get; init; } = SortDirection.Desc;
}

public enum SortDirection
{
    Asc = 1,
    Desc = 2
}