using System.Linq.Expressions;
using System.Reflection;
using Application.Repositories;
using Application.Repositories.Common;
using Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories.Common;

public static class DbSetExtensions
{
    public static async Task<PaginatedResult<T>> ToPaginatedResultAsync<T>(
        this IQueryable<T> query,
        BaseFilter filter,
        CancellationToken cancellationToken)
    {
        var totalItems = await query.CountAsync(cancellationToken);
        var totalPages = (int)Math.Ceiling(totalItems / (double)filter.PageSize);

        var items = await query
            .Skip(filter.PageSize * (filter.PageNumber - 1))
            .Take(filter.PageSize)
            .ToListAsync(cancellationToken);

        return new PaginatedResult<T>(items,
            totalItems,
            totalPages,
            filter.PageNumber,
            filter.PageSize);
    }

    public static IQueryable<T> ToSort<T>(
        this IQueryable<T> query,
        BaseFilter filter) where T : BaseEntity
    {
        var defaultOrder = query.OrderBy(e => e.Id);
        if (filter.SortByField == null)
        {
            return defaultOrder;
        }

        var property = typeof(T).GetProperty(filter.SortByField,
            BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

        if (property == null || filter.GetType().GetProperty(property.Name,
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance) == null)
        {
            throw new QueryInvalidOperation("Invalid sort field.");
        }

        var parameter = Expression.Parameter(typeof(T), "e");
        var propertyAccess = Expression.Property(parameter, property);
        var orderByExpression = Expression.Lambda(propertyAccess, parameter);

        var sortedQuery = filter.SortDirection == SortDirection.Asc
            ? Queryable.OrderBy(query, (dynamic)orderByExpression)
            : Queryable.OrderByDescending(query, (dynamic)orderByExpression);

        return sortedQuery;
    }

    public static IQueryable<T> ApplyFilter<T, TFilter>(
        this IQueryable<T> query,
        TFilter filter) where T : BaseEntity where TFilter : BaseFilter
    {
        if (filter.FilterField == null || filter.FilterValue == null)
        {
            return query;
        }

        var property = typeof(T).GetProperty(filter.FilterField,
            BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

        if (property == null || filter.GetType().GetProperty(property.Name,
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance) == null)
        {
            throw new QueryInvalidOperation("Invalid filter field.");
        }

        var parameter = Expression.Parameter(typeof(T), "e");
        var propertyAccess = Expression.Property(parameter, property);
        var constant = Expression.Constant(filter.FilterValue);

        // Handle conversion based on property type
        Expression body;
        if (property.PropertyType == typeof(string))
        {
            // Handle string properties
            if (filter.FilterOperation == Operation.Contains)
            {
                var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                body = Expression.Call(propertyAccess, containsMethod, constant);
            }
            else if (filter.FilterOperation == Operation.Equals)
            {
                body = Expression.Equal(propertyAccess, constant);
            }
            else
            {
                throw new QueryInvalidOperation("Unsupported filter operation for string type.");
            }
        }
        else if (property.PropertyType.IsEnum)
        {
            if (filter.FilterOperation != Operation.Equals)
            {
                throw new QueryInvalidOperation("Unsupported filter operation for this filterField.");
            }

            // Handle enum properties
            var parsedValue = Enum.Parse(property.PropertyType, filter.FilterValue.ToString());
            var enumConstant = Expression.Constant(parsedValue);
            body = Expression.Equal(propertyAccess, enumConstant);
        }
        else if (property.PropertyType == typeof(int))
        {
            if (filter.FilterOperation != Operation.Equals)
            {
                throw new QueryInvalidOperation("Unsupported filter operation for this filterField.");
            }

            // Handle int properties
            var convertedValue = Convert.ToInt32(filter.FilterValue);
            var intConstant = Expression.Constant(convertedValue);
            body = Expression.Equal(propertyAccess, intConstant);
        }
        else
        {
            if (filter.FilterOperation != Operation.Equals)
            {
                throw new QueryInvalidOperation("Unsupported filter operation for this filterField.");
            }

            // Handle other types (add additional cases as needed)
            constant = Expression.Constant(filter.FilterValue);
            body = Expression.Equal(propertyAccess, constant);
        }

        var predicate = Expression.Lambda<Func<T, bool>>(body, parameter);

        return query.Where(predicate);
    }
}

public class QueryInvalidOperation(string message) : Exception(message);