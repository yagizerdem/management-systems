using Microsoft.AspNetCore.Mvc;

namespace Common.DTO
{
    public class QueryFiltersDTO
    {
        [FromQuery(Name = "offset")]
        public int? Offset { get; set; }
        [FromQuery(Name = "limit")]
        public int? Limit { get; set; }

        [FromQuery(Name = "page")]
        public int? Page { get; set; }
        [FromQuery(Name = "pageSize")]
        public int? PageSize { get; set; }

        [FromQuery(Name = "filter")]
        public string? Filter { get; set; }

        [FromQuery(Name = "exclude")]
        public string? Exclude { get; set; }
        [FromQuery(Name = "select")]
        public string? Select { get; set; }

        [FromQuery(Name = "sort")]
        public string? Sort { get; set; }

        [FromQuery(Name = "search")]
        public string? Search { get; set; }

        [FromQuery(Name = "searchFields")]
        public string? SearchFields { get; set; }
    }
}