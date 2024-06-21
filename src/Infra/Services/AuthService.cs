using System.Net.Mime;
using System.Text;
using System.Text.Json;
using Application.Errors;
using Application.Errors.Common;
using Application.Services;

namespace Infra.Services;

public class AuthService(IHttpClientFactory httpClientFactory) : IAuthService
{
    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("identipass");

    public async Task<Result<LoginResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, MediaTypeNames.Application.Json);
        var response = await _httpClient.PostAsync("api/auth/login", content, cancellationToken);

        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
        if (response.IsSuccessStatusCode)
        {
            var loginResponse = JsonSerializer.Deserialize<LoginResponse>(responseContent, _jsonSerializerOptions);
            if (loginResponse is null)
            {
                return Result.Failure<LoginResponse>(AuthServiceErrors.InvalidResponse);
            }
            
            return Result.Success(loginResponse!);
        }

        var errorResponse = JsonSerializer.Deserialize<ErrorsResponse>(responseContent, _jsonSerializerOptions);

        var firstError = errorResponse!.Errors.First();

        var error = new Error(firstError.Code, firstError.Message, firstError.Type);

        return Result.Failure<LoginResponse>(error);
    }

    public async Task<Result> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken)
    {
        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, MediaTypeNames.Application.Json);
        var response = await _httpClient.PostAsync("api/auth/register", content, cancellationToken);

        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
        if (response.IsSuccessStatusCode)
        {
            return Result.Success();
        }

        var errorResponse = JsonSerializer.Deserialize<ErrorsResponse>(responseContent, _jsonSerializerOptions);
        if (errorResponse is null)
        {
            return Result.Failure(AuthServiceErrors.InvalidResponse);
        }

        var firstError = errorResponse.Errors.First();
        var error = new Error(firstError.Code, firstError.Message, firstError.Type);

        return Result.Failure(error);
    }
}