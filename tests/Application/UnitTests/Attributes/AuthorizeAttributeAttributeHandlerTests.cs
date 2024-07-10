using Application.Attributes.Authorize;
using Application.Errors;
using Application.Models.Auth;
using MediatR;

namespace Application.UnitTests.Attributes;

[TestClass]
public class AuthorizeAttributeAttributeHandlerTests
{
    private readonly ILogger _logger = Substitute.For<ILogger>();
    private readonly IRequest _request = Substitute.For<IRequest>();

    [TestMethod]
    public async Task Handle_WithValidRole_ShouldCompleteSuccessfully()
    {
        // Arrange
        var authContextMock = new AuthContext
        {
            Roles = [Role.Admin]
        };

        var handler = new AuthorizeAttributeAttributeHandler(_logger, authContextMock);
        var attribute = new AuthorizeAttribute(Role.Admin);

        // Act
        await handler.Handle(_request, attribute, default);

        // Assert
        _logger.DidNotReceiveWithAnyArgs().LogWarning(default!);
    }

    [TestMethod]
    public async Task Handle_WithInvalidRole_ShouldThrowForbiddenException()
    {
        // Arrange
        var authContextMock = new AuthContext
        {
            Roles = [Role.Admin]
        };

        var handler = new AuthorizeAttributeAttributeHandler(_logger, authContextMock);
        var attribute = new AuthorizeAttribute(Role.SuperAdmin);

        // Act & Assert
        var result = await Assert.ThrowsExceptionAsync<ResultErrorException>(async () =>
            await handler.Handle(_request, attribute, default));
        
        Assert.AreEqual(AuthErrors.Forbidden.Message, result.Error.Message);
        _logger.Received(1).LogWarning(Arg.Any<string>());
    }
}