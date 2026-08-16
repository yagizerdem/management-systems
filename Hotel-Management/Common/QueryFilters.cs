
namespace Common
{
    /// <summary>
    /// Query parameters for API filtering, pagination, sorting, and field selection
    /// </summary>
    public class QueryFilters
    {
        /// <summary>
        /// Number of records to skip (for pagination)
        /// </summary>
        public int? Offset { get; set; }

        /// <summary>
        /// Maximum number of records to return (for pagination)
        /// </summary>
        public int? Limit { get; set; }

        /// <summary>
        /// Page number (alternative to offset, 1-based)
        /// </summary>
        public int? Page { get; set; }

        /// <summary>
        /// Page size (alternative to limit)
        /// </summary>
        public int? PageSize { get; set; }

        /// <summary>
        /// Filter expression (e.g., "name eq 'John' and age gt 25")
        /// Supported operators: eq, ne, gt, gte, lt, lte, contains, startswith, endswith
        /// </summary>
        public string? Filter { get; set; }

        /// <summary>
        /// Comma-separated list of fields to exclude from the result
        /// </summary>
        public string? Exclude { get; set; }

        /// <summary>
        /// Comma-separated list of fields to include in the result (select only these)
        /// </summary>
        public string? Select { get; set; }

        /// <summary>
        /// Sort field and direction (e.g., "name", "-createdAt" for descending)
        /// Comma-separated for multiple sorts: "name,-createdAt"
        /// </summary>
        public string? Sort { get; set; }

        /// <summary>
        /// Search term for full-text search across specified fields
        /// </summary>
        public string? Search { get; set; }

        /// <summary>
        /// Comma-separated list of fields to search in (used with Search)
        /// </summary>
        public string? SearchFields { get; set; }

        /// <summary>
        /// Get excluded fields as a list
        /// </summary>
        public List<string> GetExcludedFields()
        {
            if (string.IsNullOrWhiteSpace(Exclude))
                return new List<string>();

            return Exclude.Split(',', StringSplitOptions.RemoveEmptyEntries)
                         .Select(f => f.Trim())
                         .ToList();
        }

        /// <summary>
        /// Get selected fields as a list
        /// </summary>
        public List<string> GetSelectedFields()
        {
            if (string.IsNullOrWhiteSpace(Select))
                return new List<string>();

            return Select.Split(',', StringSplitOptions.RemoveEmptyEntries)
                         .Select(f => f.Trim())
                         .ToList();
        }

        /// <summary>
        /// Get sort fields as a list of tuples (fieldName, isDescending)
        /// </summary>
        public List<(string Field, bool IsDescending)> GetSortFields()
        {
            if (string.IsNullOrWhiteSpace(Sort))
                return new List<(string, bool)>();

            return Sort.Split(',', StringSplitOptions.RemoveEmptyEntries)
                      .Select(s => s.Trim())
                      .Select(s =>
                      {
                          bool isDesc = s.StartsWith("-");
                          string field = isDesc ? s.Substring(1) : s;
                          return (field, isDesc);
                      })
                      .ToList();
        }

        /// <summary>
        /// Get search fields as a list
        /// </summary>
        public List<string> GetSearchFields()
        {
            if (string.IsNullOrWhiteSpace(SearchFields))
                return new List<string>();

            return SearchFields.Split(',', StringSplitOptions.RemoveEmptyEntries)
                              .Select(f => f.Trim())
                              .ToList();
        }

        /// <summary>
        /// Calculate offset from page and pageSize
        /// </summary>
        public int GetOffset()
        {
            if (Offset.HasValue)
                return Offset.Value;

            if (Page.HasValue && PageSize.HasValue)
                return (Page.Value - 1) * PageSize.Value;

            return 0;
        }

        /// <summary>
        /// Get limit value (prioritizes Limit over PageSize)
        /// </summary>
        public int? GetLimit()
        {
            return Limit ?? PageSize;
        }
    }
}
