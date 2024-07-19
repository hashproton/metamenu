namespace Infra.Identity.Services.Requests;

public class LoginRequest
{
    public string EmailOrUsername { get; set; } = null!;
    
    public string Password { get; set; } = null!;
}