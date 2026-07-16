namespace CodeYield.Persistence
{
    /// <summary>
    /// Represents a page of results from a paginated query, including metadata
    /// for building navigation controls.
    /// </summary>
    /// <typeparam name="T">The type of items in the page.</typeparam>
    public class PaginatedResult<T>
    {
        /// <summary>Gets the items in the current page.</summary>
        public IReadOnlyList<T> Items { get; }

        /// <summary>Gets the current page number (1-based).</summary>
        public int Page { get; }

        /// <summary>Gets the number of items per page.</summary>
        public int PageSize { get; }

        /// <summary>Gets the total number of items across all pages.</summary>
        public int TotalCount { get; }

        /// <summary>Gets the total number of pages.</summary>
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

        /// <summary>Gets whether there is a previous page.</summary>
        public bool HasPrevious => Page > 1;

        /// <summary>Gets whether there is a next page.</summary>
        public bool HasNext => Page < TotalPages;

        /// <summary>Initializes a new instance of <see cref="PaginatedResult{T}"/>.</summary>
        public PaginatedResult(IReadOnlyList<T> items, int page, int pageSize, int totalCount)
        {
            Items = items;
            Page = page;
            PageSize = pageSize;
            TotalCount = totalCount;
        }
    }
}
