using System.Linq.Expressions;
using System.Reflection;
using Application.Repositories.Common;
using Domain.Common;
using Microsoft.EntityFrameworkCore;
using QueryKit;
using QueryKit.Configuration;

namespace Infra.Repositories.Common;

internal class GenericRepository<T>(
    AppDbContext context) : IGenericRepository<T> where T : BaseEntity
{
    public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await context.Set<T>().FindAsync([id], cancellationToken);
    }

    public Task<T?> GetByQueryAsync(Expression<Func<T, bool>> query, CancellationToken cancellationToken)
    {
        return context.Set<T>().FirstOrDefaultAsync(query, cancellationToken);
    }

    public Task<int> CountAsync(CancellationToken cancellationToken)
    {
        return context.Set<T>().CountAsync(cancellationToken);
    }

    public async Task<PaginatedResult<T>> GetAllAsync<TFilter>(
        TFilter filter,
        CancellationToken cancellationToken) where TFilter : BaseFilter
    {
        return await context.Set<T>()
            .ApplyQueryKitFilter(filter.FilterTerm ?? "", BuildQueryKitConfiguration<TFilter>())
            .ApplyQueryKitSort(filter.SortTerm ?? "")
            .ToPaginatedResultAsync(filter, cancellationToken);
    }

    public async Task<int> AddAsync(T entity, CancellationToken cancellationToken)
    {
        var entry = context.Set<T>().Add(entity);

        await context.SaveChangesAsync(cancellationToken);

        return entry.Entity.Id;
    }

    public async Task UpdateAsync(T entity, CancellationToken cancellationToken)
    {
        context.Set<T>().Update(entity);

        await context.SaveChangesAsync(cancellationToken);
    }

    public Task DeleteAsync(T entity, CancellationToken cancellationToken)
    {
        context.Set<T>().Remove(entity);

        return context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Constructs a QueryKitConfiguration instance tailored to the specific filter type <typeparamref name="TFilter"/>.
    /// This configuration ensures that only properties defined in the filter type are considered for dynamic filtering,
    /// effectively ignoring any properties of the entity <typeparamref name="T"/> that are not present in the filter.
    /// </summary>
    /// <typeparam name="TFilter">The type of the filter, which determines the properties available for filtering.</typeparam>
    /// <returns>A QueryKitConfiguration object configured to restrict filtering to the properties defined in <typeparamref name="TFilter"/>.</returns>
    /// <remarks>
    /// This method leverages reflection to identify properties of <typeparamref name="TFilter"/> and compares them with
    /// the properties of the entity type <typeparamref name="T"/>. Properties not present in the filter type are explicitly
    /// prevented from being used in filtering operations, enhancing security and performance by avoiding unintended data exposure
    /// and reducing the complexity of generated queries.
    /// </remarks>
    private static QueryKitConfiguration BuildQueryKitConfiguration<TFilter>()
    {
        return new QueryKitConfiguration(op =>
        {
            var filterProperties = typeof(TFilter)
                .GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);

            var entityProperties = typeof(T)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var preventFilterProperties = entityProperties
                .Where(fp => !filterProperties.Any(p => p.Name.Equals(fp.Name, StringComparison.OrdinalIgnoreCase)));

            foreach (var preventFilterProperty in preventFilterProperties)
            {
                var param = Expression.Parameter(typeof(T), "t");
                var property = Expression.Property(param, preventFilterProperty.Name);
                var convert = Expression.Convert(property, typeof(object));

                var expression = Expression.Lambda<Func<T, object>>(convert, param);

                var method = typeof(QueryKitSettings).GetMethod("Property")!.MakeGenericMethod(typeof(T));
                var result = method.Invoke(op, [expression]);

                var preventFilterMethod = result!.GetType().GetMethod("PreventFilter");
                preventFilterMethod!.Invoke(result, null);
            }
        });
    }
}