using Application.UseCases.Tenants.Commands;

namespace Application.Services;

public class LoginRequest
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}

public class LoginResponse
{
    public string Token { get; set; }
    
    public string RefreshToken { get; set; }
}

public class RegisterRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
}

public class ErrorsResponse
{
    public List<Error> Errors { get; set; }
}

public class GetMeResponse
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public List<Role> Roles { get; set; }

    public List<int> TenantIds { get; set; }
}

public interface IAuthService
{
    Task<Result<LoginResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken);
    
    Task<Result> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken);
    Task<Result<GetMeResponse>> GetMeAsync(string token, string refreshToken,  CancellationToken cancellationToken);
}