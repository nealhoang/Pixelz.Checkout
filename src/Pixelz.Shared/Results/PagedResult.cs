namespace Pixelz.Shared.Results;

/// <summary>
/// Represents a generic paginated result set, typically used for API responses
/// or query results where data is returned in pages.
/// </summary>
/// <typeparam name="T">The type of items contained in the result.</typeparam>
public class PagedResult<T>
{
    /// <summary>
    /// Gets or sets the list of items for the current page.
    /// </summary>
    public List<T> Items { get; set; } = new();

    /// <summary>
    /// Gets or sets the total number of records across all pages.
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Gets or sets the index of the current page (1-based).
    /// </summary>
    public int PageIndex { get; set; }

    /// <summary>
    /// Gets or sets the size of a single page (number of records per page).
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Gets the total number of pages calculated from <see cref="TotalCount"/> and <see cref="PageSize"/>.
    /// </summary>
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

    /// <summary>
    /// Indicates whether there is a previous page available.
    /// </summary>
    public bool HasPreviousPage => PageIndex > 1;

    /// <summary>
    /// Indicates whether there is a next page available.
    /// </summary>
    public bool HasNextPage => PageIndex < TotalPages;

    /// <summary>
    /// Initializes a new instance of the <see cref="PagedResult{T}"/> class.
    /// </summary>
    public PagedResult() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="PagedResult{T}"/> class with the specified values.
    /// </summary>
    /// <param name="items">The list of items for the current page.</param>
    /// <param name="totalCount">The total number of records across all pages.</param>
    /// <param name="pageIndex">The index of the current page (1-based).</param>
    /// <param name="pageSize">The size of a single page.</param>
    public PagedResult(List<T> items, int totalCount, int pageIndex, int pageSize)
    {
        Items = items;
        TotalCount = totalCount;
        PageIndex = pageIndex;
        PageSize = pageSize;
    }
}
