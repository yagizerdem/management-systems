using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;

namespace Common
{
    /// <summary>
    /// Provides advanced querying capabilities for API endpoints including
    /// filtering, pagination, sorting, field selection, and searching
    /// </summary>
    public class ApiFeatures<T> where T : class
    {
        private IQueryable<T> _query;
        private readonly QueryFilters _queryFilters;
        private readonly int _maxLimit;
        private readonly int _defaultLimit;

        public ApiFeatures(
            IQueryable<T> queryable,
            QueryFilters queryFilters,
            int maxLimit = 1000,
            int defaultLimit = 100)
        {
            _query = queryable ?? throw new ArgumentNullException(nameof(queryable));
            _queryFilters = queryFilters ?? new QueryFilters();
            _maxLimit = maxLimit;
            _defaultLimit = defaultLimit;
        }

        /// <summary>
        /// Apply all query features and return the result
        /// </summary>
        public IQueryable<T> ApplyAll()
        {
            ApplyFilter();
            ApplySearch();
            ApplySort();
            return _query;
        }

        /// <summary>
        /// Apply all features and return paginated result
        /// </summary>
        public async Task<PagedResult<T>> ApplyAllWithPaginationAsync()
        {
            ApplyFilter();
            ApplySearch();

            int totalCount = _query.Count();

            ApplySort();
            ApplyPagination();

            var items = _query.ToList();

            int offset = _queryFilters.GetOffset();
            int limit = GetEffectiveLimit();

            return new PagedResult<T>
            {
                Items = items,
                TotalCount = totalCount,
                Offset = offset,
                Limit = limit,
                Page = _queryFilters.Page ?? (offset / limit) + 1,
                PageSize = limit,
                TotalPages = (int)Math.Ceiling(totalCount / (double)limit),
                HasNextPage = offset + limit < totalCount,
                HasPreviousPage = offset > 0
            };
        }

        /// <summary>
        /// Apply filtering based on filter expression
        /// Example: "name eq 'John' and age gt 25"
        /// </summary>
        public ApiFeatures<T> ApplyFilter()
        {
            if (string.IsNullOrWhiteSpace(_queryFilters.Filter))
                return this;

            try
            {
                var filterExpression = ParseFilterExpression(_queryFilters.Filter);
                if (filterExpression != null)
                {
                    _query = _query.Where(filterExpression);
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Invalid filter expression: {ex.Message}", ex);
            }

            return this;
        }

        /// <summary>
        /// Apply search across specified fields
        /// </summary>
        public ApiFeatures<T> ApplySearch()
        {
            if (string.IsNullOrWhiteSpace(_queryFilters.Search))
                return this;

            var searchFields = _queryFilters.GetSearchFields();
            if (searchFields.Count == 0)
            {
                // Search all string properties by default
                searchFields = typeof(T).GetProperties()
                    .Where(p => p.PropertyType == typeof(string))
                    .Select(p => p.Name)
                    .ToList();
            }

            if (searchFields.Count == 0)
                return this;

            var searchTerm = _queryFilters.Search.ToLower();
            var parameter = Expression.Parameter(typeof(T), "x");
            Expression? combinedExpression = null;

            foreach (var fieldName in searchFields)
            {
                var property = typeof(T).GetProperty(fieldName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (property == null || property.PropertyType != typeof(string))
                    continue;

                var propertyAccess = Expression.Property(parameter, property);
                var toLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
                var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });

                var toLowerCall = Expression.Call(propertyAccess, toLowerMethod!);
                var searchTermExpression = Expression.Constant(searchTerm);
                var containsCall = Expression.Call(toLowerCall, containsMethod!, searchTermExpression);

                combinedExpression = combinedExpression == null
                    ? containsCall
                    : Expression.OrElse(combinedExpression, containsCall);
            }

            if (combinedExpression != null)
            {
                var lambda = Expression.Lambda<Func<T, bool>>(combinedExpression, parameter);
                _query = _query.Where(lambda);
            }

            return this;
        }

        /// <summary>
        /// Apply sorting
        /// Example: "name" or "-createdAt" for descending
        /// </summary>
        public ApiFeatures<T> ApplySort()
        {
            var sortFields = _queryFilters.GetSortFields();
            if (sortFields.Count == 0)
                return this;

            IOrderedQueryable<T>? orderedQuery = null;

            foreach (var (field, isDescending) in sortFields)
            {
                var property = typeof(T).GetProperty(field, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (property == null)
                    continue;

                var parameter = Expression.Parameter(typeof(T), "x");
                var propertyAccess = Expression.Property(parameter, property);
                var lambda = Expression.Lambda(propertyAccess, parameter);

                var methodName = orderedQuery == null
                    ? (isDescending ? "OrderByDescending" : "OrderBy")
                    : (isDescending ? "ThenByDescending" : "ThenBy");

                var orderByMethod = typeof(Queryable).GetMethods()
                    .First(m => m.Name == methodName && m.GetParameters().Length == 2)
                    .MakeGenericMethod(typeof(T), property.PropertyType);

                orderedQuery = (IOrderedQueryable<T>)orderByMethod.Invoke(null, new object[] { orderedQuery ?? _query, lambda })!;
            }

            if (orderedQuery != null)
                _query = orderedQuery;

            return this;
        }

        /// <summary>
        /// Apply pagination (offset and limit)
        /// </summary>
        public ApiFeatures<T> ApplyPagination()
        {
            int offset = _queryFilters.GetOffset();
            int limit = GetEffectiveLimit();

            _query = _query.Skip(offset).Take(limit);
            return this;
        }

        /// <summary>
        /// Apply offset only
        /// </summary>
        public ApiFeatures<T> ApplyOffset()
        {
            int offset = _queryFilters.GetOffset();
            if (offset > 0)
                _query = _query.Skip(offset);
            return this;
        }

        /// <summary>
        /// Apply limit only
        /// </summary>
        public ApiFeatures<T> ApplyLimit()
        {
            int limit = GetEffectiveLimit();
            _query = _query.Take(limit);
            return this;
        }

        /// <summary>
        /// Select specific fields (returns dynamic objects)
        /// </summary>
        public IQueryable<dynamic> ApplySelect()
        {
            var selectedFields = _queryFilters.GetSelectedFields();
            if (selectedFields.Count == 0)
                return _query;

            var properties = typeof(T).GetProperties()
                .Where(p => selectedFields.Contains(p.Name, StringComparer.OrdinalIgnoreCase))
                .ToList();

            if (properties.Count == 0)
                return _query;

            var parameter = Expression.Parameter(typeof(T), "x");
            var bindings = properties.Select(p =>
                Expression.Bind(
                    typeof(T).GetProperty(p.Name)!,
                    Expression.Property(parameter, p)
                )
            );

            var memberInit = Expression.MemberInit(Expression.New(typeof(T)), bindings);
            var lambda = Expression.Lambda<Func<T, dynamic>>(memberInit, parameter);

            return _query.Select(lambda);
        }

        /// <summary>
        /// Exclude specific fields (returns objects without excluded fields)
        /// Note: This creates a new anonymous type
        /// </summary>
        public IQueryable<dynamic> ApplyExclude()
        {
            var excludedFields = _queryFilters.GetExcludedFields();
            if (excludedFields.Count == 0)
                return _query;

            var properties = typeof(T).GetProperties()
                .Where(p => !excludedFields.Contains(p.Name, StringComparer.OrdinalIgnoreCase))
                .ToList();

            if (properties.Count == 0)
                return _query;

            var parameter = Expression.Parameter(typeof(T), "x");
            var bindings = properties.Select(p =>
                Expression.Bind(
                    typeof(T).GetProperty(p.Name)!,
                    Expression.Property(parameter, p)
                )
            );

            var memberInit = Expression.MemberInit(Expression.New(typeof(T)), bindings);
            var lambda = Expression.Lambda<Func<T, dynamic>>(memberInit, parameter);

            return _query.Select(lambda);
        }

        /// <summary>
        /// Get the current query
        /// </summary>
        public IQueryable<T> GetQuery() => _query;

        /// <summary>
        /// Get effective limit (respects max limit)
        /// </summary>
        private int GetEffectiveLimit()
        {
            int requestedLimit = _queryFilters.GetLimit() ?? _defaultLimit;
            return Math.Min(requestedLimit, _maxLimit);
        }

        /// <summary>
        /// Parse filter expression into LINQ expression
        /// Supports: eq, ne, gt, gte, lt, lte, contains, startswith, endswith
        /// Example: "name eq 'John' and age gt 25"
        /// </summary>
        private Expression<Func<T, bool>>? ParseFilterExpression(string filter)
        {
            if (string.IsNullOrWhiteSpace(filter))
                return null;

            var parameter = Expression.Parameter(typeof(T), "x");
            var expression = ParseFilterPart(filter, parameter);

            if (expression == null)
                return null;

            return Expression.Lambda<Func<T, bool>>(expression, parameter);
        }

        private Expression? ParseFilterPart(string filter, ParameterExpression parameter)
        {
            // Handle 'and' operator
            if (filter.Contains(" and ", StringComparison.OrdinalIgnoreCase))
            {
                var parts = filter.Split(new[] { " and " }, StringSplitOptions.RemoveEmptyEntries);
                Expression? combined = null;

                foreach (var part in parts)
                {
                    var partExpr = ParseFilterPart(part.Trim(), parameter);
                    if (partExpr != null)
                    {
                        combined = combined == null ? partExpr : Expression.AndAlso(combined, partExpr);
                    }
                }

                return combined;
            }

            // Handle 'or' operator
            if (filter.Contains(" or ", StringComparison.OrdinalIgnoreCase))
            {
                var parts = filter.Split(new[] { " or " }, StringSplitOptions.RemoveEmptyEntries);
                Expression? combined = null;

                foreach (var part in parts)
                {
                    var partExpr = ParseFilterPart(part.Trim(), parameter);
                    if (partExpr != null)
                    {
                        combined = combined == null ? partExpr : Expression.OrElse(combined, partExpr);
                    }
                }

                return combined;
            }

            // Parse single condition
            return ParseSingleCondition(filter, parameter);
        }

        private Expression? ParseSingleCondition(string condition, ParameterExpression parameter)
        {
            var operators = new[] { " eq ", " ne ", " gt ", " gte ", " lt ", " lte ", " contains ", " startswith ", " endswith " };

            foreach (var op in operators)
            {
                if (condition.Contains(op, StringComparison.OrdinalIgnoreCase))
                {
                    var parts = condition.Split(new[] { op }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length != 2)
                        continue;

                    var fieldName = parts[0].Trim();
                    var value = parts[1].Trim().Trim('\'', '"');

                    var property = typeof(T).GetProperty(fieldName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    if (property == null)
                        continue;

                    var propertyAccess = Expression.Property(parameter, property);
                    var constantValue = ConvertValue(value, property.PropertyType);
                    var constant = Expression.Constant(constantValue);

                    return op.Trim().ToLower() switch
                    {
                        "eq" => Expression.Equal(propertyAccess, constant),
                        "ne" => Expression.NotEqual(propertyAccess, constant),
                        "gt" => Expression.GreaterThan(propertyAccess, constant),
                        "gte" => Expression.GreaterThanOrEqual(propertyAccess, constant),
                        "lt" => Expression.LessThan(propertyAccess, constant),
                        "lte" => Expression.LessThanOrEqual(propertyAccess, constant),
                        "contains" => Expression.Call(propertyAccess, typeof(string).GetMethod("Contains", new[] { typeof(string) })!, constant),
                        "startswith" => Expression.Call(propertyAccess, typeof(string).GetMethod("StartsWith", new[] { typeof(string) })!, constant),
                        "endswith" => Expression.Call(propertyAccess, typeof(string).GetMethod("EndsWith", new[] { typeof(string) })!, constant),
                        _ => null
                    };
                }
            }

            return null;
        }

        private object? ConvertValue(string value, Type targetType)
        {
            if (targetType == typeof(string))
                return value;

            var underlyingType = Nullable.GetUnderlyingType(targetType) ?? targetType;

            if (underlyingType.IsEnum)
                return Enum.Parse(underlyingType, value, true);

            return Convert.ChangeType(value, underlyingType);
        }
    }
}
