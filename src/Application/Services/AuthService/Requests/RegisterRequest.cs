namespace Application.Services.AuthService.Requests;

public class RegisterRequest
{
    public string Username { get; set; } = default!;

    public string Password { get; set; } = default!;

    public string ConfirmPassword { get; set; } = default!;
}