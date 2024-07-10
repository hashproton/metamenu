using Application.Attributes.TenantClaim;
using Application.Errors;
using Application.Models.Auth;
using MediatR;

namespace Application.UnitTests.Attributes;

public record MockTenantCommand(
    int? TenantId) : IRequest;

[TestClass]
public class TenantClaimAttributeHandlerTests
{
    private readonly ILogger _logger = Substitute.For<ILogger>();

    private MockTenantCommand? _request;

    [TestMethod]
    public async Task Handle_WithValidTenantId_ShouldCompleteSuccessfully()
    {
        // Arrange
        var authContextMock = new AuthContext
        {
            UserId = Guid.NewGuid(),
            TenantIds = [ 1, 2, 3 ] // Assuming valid tenant ids
        };

        var handler = new TenantClaimAttributeHandler(_logger, authContextMock);
        var attribute = new TenantClaimAttribute();

        _request = new MockTenantCommand(2);

        // Act
        await handler.Handle(_request, attribute, CancellationToken.None);

        // Assert
        _logger.DidNotReceiveWithAnyArgs().LogWarning(default!);
    }

    [TestMethod]
    public async Task Handle_WithMissingTenantId_ShouldThrowUnauthorizedException()
    {
        // Arrange
        var authContextMock = new AuthContext
        {
            UserId = Guid.NewGuid(),
        };

        var handler = new TenantClaimAttributeHandler(_logger, authContextMock);
        var attribute = new TenantClaimAttribute();

        _request = new MockTenantCommand(null);

        // Act & Assert
        var result = await Assert.ThrowsExceptionAsync<ResultErrorException>(async () =>
            await handler.Handle(_request, attribute, CancellationToken.None));
        
        Assert.AreEqual(AuthErrors.Unauthorized.Message, result.Error.Message);
        _logger.Received(1).LogWarning(Arg.Any<string>());
    }

    [TestMethod]
    public async Task Handle_WithInvalidTenantId_ShouldThrowForbiddenException()
    {
        // Arrange
        var authContextMock = new AuthContext
        {
            UserId = Guid.NewGuid(),
            TenantIds = [1, 2, 3]
        };

        var handler = new TenantClaimAttributeHandler(_logger, authContextMock);
        var attribute = new TenantClaimAttribute();
        
        _request = new MockTenantCommand(999);

        // Act & Assert
        var result = await Assert.ThrowsExceptionAsync<ResultErrorException>(async () =>
            await handler.Handle(_request, attribute, CancellationToken.None));
        
        Assert.AreEqual(AuthErrors.Forbidden.Message, result.Error.Message);
        _logger.Received(1).LogWarning(Arg.Any<string>());
    }
}