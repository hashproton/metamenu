using System.Reflection;
using Application.Attributes;
using Application.Services;
using Application.UseCases.Tenants.Commands;
using MediatR.Pipeline;

namespace Application.Mediator.PreProcessors;

public class RolesPreProcessor<TRequest>(
    ILogger logger,
    AuthContext authContext) : IRequestPreProcessor<TRequest> where TRequest : notnull
{
    public Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var authorizeAttribute = typeof(TRequest)
            .GetCustomAttribute<AuthorizeAttribute>();

        if (authorizeAttribute is null)
        {
            return Task.CompletedTask;
        }

        var roles = authorizeAttribute.GetType()
            .GetProperties()
            .Select(p => p.GetValue(authorizeAttribute))
            .OfType<Role>()
            .ToList();

        if (roles.Count == 0)
        {
            return Task.CompletedTask;
        }

        if (!roles.Any(r => authContext.Roles.Contains(r)))
        {
            var roleNames = string.Join(", ", roles.Select(r => r.ToString()));
            logger.LogWarning(
                $"User {authContext.UserId} tried to access {typeof(TRequest).Name} without required roles: {roleNames}");
            throw new ResultErrorException(AuthErrors.Forbidden);
        }

        return Task.CompletedTask;
    }
}

public class AttributeHandlerPreProcessor<TRequest>(
    ILogger logger,
    IServiceProvider serviceProvider) : IRequestPreProcessor<TRequest> where TRequest : notnull
{
    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var attributes = typeof(TRequest)
            .GetCustomAttributes<Attribute>()
            .Where(at => at.GetType().Assembly == typeof(DependencyInjection).Assembly)
            .ToList();

        if (!attributes.Any())
        {
            return;
        }

        foreach (var attribute in attributes)
        {
            var handlerType = typeof(IAttributeHandler<>).MakeGenericType(attribute.GetType());
            var handler = serviceProvider.GetService(handlerType);

            if (handler is null)
            {
                throw new InvalidOperationException($"No handler found for attribute {attribute.GetType().Name}");
            }

            var handleMethod = handlerType.GetMethod("Handle");
            if (handleMethod != null)
            {
                var genericHandleMethod = handleMethod.MakeGenericMethod(typeof(TRequest));
                try
                {
                    var handleTask = (Task)genericHandleMethod.Invoke(handler,
                        [request, attribute, cancellationToken])!;
                    await handleTask.ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    logger.LogError($"Error handling attribute {attribute.GetType().Name}");
                    throw; // Rethrow the exception to propagate it correctly
                }
            }
        }
    }
}

public interface IAttributeHandler<in TAttribute> where TAttribute : Attribute
{
    Task Handle<TRequest>(
        TRequest request,
        TAttribute attribute,
        CancellationToken cancellationToken) where TRequest : notnull;
}

public class TenantClaimAttributeHandler(
    ILogger logger,
    AuthContext authContext) : IAttributeHandler<TenantClaimAttribute>
{
    public Task Handle<TRequest>(
        TRequest request,
        TenantClaimAttribute attribute,
        CancellationToken cancellationToken) where TRequest : notnull
    {
        var tenantId = request.GetType()
            .GetProperty("TenantId")
            ?.GetValue(request);

        if (tenantId is null)
        {
            logger.LogWarning($"User {authContext.UserId} tried to access {typeof(TRequest).Name} without tenant id");

            throw new ResultErrorException(AuthErrors.Unauthorized);
        }

        if (!authContext.TenantIds.Contains((int)tenantId))
        {
            logger.LogWarning(
                $"User {authContext.UserId} tried to access {typeof(TRequest).Name} with invalid tenant id {tenantId}");

            throw new ResultErrorException(AuthErrors.Forbidden);
        }

        return Task.CompletedTask;
    }
}