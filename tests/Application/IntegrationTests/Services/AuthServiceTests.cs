using Application.Errors.Common;
using Application.Services.AuthService;
using Application.Services.AuthService.Requests;
using Infra.Services.AuthService;
using NSubstitute;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace Application.IntegrationTests.Services;

[TestClass]
public class AuthServiceTests
{
    private IAuthService _authService = null!;
    private WireMockServer _server = null!;

    [TestInitialize]
    public void SetUp()
    {
        _server = WireMockServer.Start();

        var httpClientFactory = Substitute.For<IHttpClientFactory>();
        var httpClient = new HttpClient
        {
            BaseAddress = new Uri($"{_server.Urls[0]}")
        };
        httpClientFactory.CreateClient("identipass").Returns(httpClient);

        _authService = new AuthService(httpClientFactory);
    }

    [TestMethod]
    public async Task LoginAsync_ShouldReturnSuccess_WhenCredentialsAreValid()
    {
        // Arrange
        var request = new LoginRequest("test", "test@gmail.com", "password");

        _server.Given(Request.Create().WithPath("/api/auth/login").UsingPost())
            .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithHeader("Content-Type", "application/json")
                .WithBodyAsJson(new
                {
                    token = "mockToken",
                    refreshToken = "mockRefresh"
                }));

        // Act
        var result = await _authService.LoginAsync(request, default);

        // Assert
        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual("mockToken", result.Value.Token);
    }

    [TestMethod]
    public async Task LoginAsync_ShouldReturnFailure_WhenCredentialsAreInvalid()
    {
        // Arrange
        var request = new LoginRequest("test", "email@test.com", "password");

        _server.Given(Request.Create().WithPath("/api/auth/login").UsingPost())
            .RespondWith(Response.Create()
                .WithStatusCode(400)
                .WithHeader("Content-Type", "application/json")
                .WithBodyAsJson(new
                {
                    errors = new[]
                    {
                        new
                        {
                            code = "invalid_credentials",
                            message = "Invalid credentials",
                            type = ErrorType.Conflict
                        }
                    }
                }));

        // Act
        var result = await _authService.LoginAsync(request, default);

        // Assert
        Assert.IsFalse(result.IsSuccess);

        Assert.IsNotNull(result.Error);
        Assert.AreEqual("Invalid credentials", result.Error.Message);
        Assert.AreEqual("invalid_credentials", result.Error.Code);
    }
}