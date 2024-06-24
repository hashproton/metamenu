namespace Application.Services.AuthService.Responses;

public class LoginResponse
{
    public string Token { get; set; } = default!;

    public string RefreshToken { get; set; } = default!;
}