using Api.Extensions;
using Application.Services;

namespace Api.Controllers;

public class AuthController(
    IAuthService authService) : BaseController
{
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
    {
        var result = await authService.LoginAsync(request, CancellationToken.None);

        return result.IsSuccess ? Ok(result.Value) : result.ToActionResult();
    }
    
    [HttpPost("register")]
    public async Task<ActionResult<LoginResponse>> Register([FromBody] RegisterRequest request)
    {
        var result = await authService.RegisterAsync(request, CancellationToken.None);

        return result.IsSuccess ? NoContent() : result.ToActionResult();
    }
}