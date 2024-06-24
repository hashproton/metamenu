namespace Application.Attributes.Common;

public interface IAttributeHandler<in TAttribute> where TAttribute : Attribute
{
    Task Handle<TRequest>(
        TRequest request,
        TAttribute attribute,
        CancellationToken cancellationToken) where TRequest : notnull;
}