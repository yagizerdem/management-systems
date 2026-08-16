using System;
using System.Collections.Generic;

namespace Common
{
    /// <summary>
    /// Represents a paginated result set
    /// </summary>
    public class PagedResult<T>
    {
        /// <summary>
        /// The items in the current page
        /// </summary>
        public List<T> Items { get; set; } = new List<T>();

        /// <summary>
        /// Total number of items across all pages
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Number of items skipped (offset)
        /// </summary>
        public int Offset { get; set; }

        /// <summary>
        /// Maximum number of items per page (limit)
        /// </summary>
        public int Limit { get; set; }

        /// <summary>
        /// Current page number (1-based)
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// Number of items per page
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Total number of pages
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// Whether there is a next page
        /// </summary>
        public bool HasNextPage { get; set; }

        /// <summary>
        /// Whether there is a previous page
        /// </summary>
        public bool HasPreviousPage { get; set; }

        /// <summary>
        /// Number of items in the current page
        /// </summary>
        public int Count => Items.Count;

        /// <summary>
        /// Whether this is the first page
        /// </summary>
        public bool IsFirstPage => Page == 1;

        /// <summary>
        /// Whether this is the last page
        /// </summary>
        public bool IsLastPage => Page == TotalPages;
    }

    /// <summary>
    /// Extension methods for creating paged results
    /// </summary>
    public static class PagedResultExtensions
    {
        /// <summary>
        /// Convert a PagedResult to an API response
        /// </summary>
        public static ApiResponse<PagedResult<T>> ToApiResponse<T>(this PagedResult<T> pagedResult)
        {
            return ApiResponse<PagedResult<T>>.Ok(pagedResult);
        }

        /// <summary>
        /// Map items to a different type
        /// </summary>
        public static PagedResult<TDestination> Map<TSource, TDestination>(
            this PagedResult<TSource> source,
            Func<TSource, TDestination> mapper)
        {
            return new PagedResult<TDestination>
            {
                Items = source.Items.Select(mapper).ToList(),
                TotalCount = source.TotalCount,
                Offset = source.Offset,
                Limit = source.Limit,
                Page = source.Page,
                PageSize = source.PageSize,
                TotalPages = source.TotalPages,
                HasNextPage = source.HasNextPage,
                HasPreviousPage = source.HasPreviousPage
            };
        }
    }
}
