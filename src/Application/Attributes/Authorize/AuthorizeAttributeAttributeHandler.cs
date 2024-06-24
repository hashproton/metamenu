using Application.Attributes.Common;
using Application.Models;
using Application.Services;

namespace Application.Attributes.Authorize;

public class AuthorizeAttributeAttributeHandler(
    ILogger logger,
    AuthContext authContext) : IAttributeHandler<AuthorizeAttribute>
{
    public Task Handle<TRequest>(
        TRequest request,
        AuthorizeAttribute attribute,
        CancellationToken cancellationToken) where TRequest : notnull
    {
        if (!authContext.Roles.Contains(attribute.Role))
        {
            logger.LogWarning(
                $"User {authContext.UserId} tried to access {typeof(TRequest).Name} without required role {attribute.Role}");

            throw new ResultErrorException(AuthErrors.Forbidden);
        }

        return Task.CompletedTask;
    }
}