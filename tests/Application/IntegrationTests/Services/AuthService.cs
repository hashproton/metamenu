using Application.Models.Auth;
using Application.Services.AuthService;
using Application.Services.AuthService.Requests;
using Infra.Services.AuthService;
using NSubstitute;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using WireMock.Settings;
using WireMock.Types;

namespace Application.IntegrationTests.Services;

[TestClass]
public class AuthServiceTests
{
    private HttpClient _httpClient;
    private IAuthService _authService;
    private WireMockServer _server;

    [TestInitialize]
    public void SetUp()
    {
        _server = WireMockServer.Start();

        var httpClientFactory = Substitute.For<IHttpClientFactory>();
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri($"{_server.Urls[0]}")
        };
        httpClientFactory.CreateClient("identipass").Returns(_httpClient);

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
        var result = await _authService.LoginAsync(request, CancellationToken.None);

        // Assert
        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual("mockToken", result.Value.Token);
    }
}