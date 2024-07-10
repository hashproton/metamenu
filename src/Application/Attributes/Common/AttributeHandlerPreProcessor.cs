using System.Reflection;
using Application.Services;
using MediatR.Pipeline;

namespace Application.Attributes.Common;

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

                    if (ex.InnerException is not null)
                    {
                        throw ex.InnerException;
                    }

                    throw;
                }
            }
        }
    }
}