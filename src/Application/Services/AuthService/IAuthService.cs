using Application.Services.AuthService.Requests;
using Application.Services.AuthService.Responses;

namespace Application.Services.AuthService;

public interface IAuthService
{
    Task<Result<LoginResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken);

    Task<Result> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken);

    Task<Result<GetMeResponse>> GetMeAsync(
        string token,
        string refreshToken,
        CancellationToken cancellationToken);
}