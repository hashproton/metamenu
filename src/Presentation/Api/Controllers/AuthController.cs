using Api.Extensions;
using Application.Errors.Common;
using Infra.Identity.Services;
using Infra.Identity.Services.Requests;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Api.Controllers;

public class AuthController(
    IAuthService authService) : BaseController
{
    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] RegisterRequest request)
    {
        var result = await authService.RegisterAsync(request);
        
        return result.IsSuccess
            ? Created()
            : result.ToActionResult();
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await authService.LoginAsync(request);
        
        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToActionResult();
    }
    
    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<Result<GetMeResponse>>> Me()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null || !Guid.TryParse(userId, out var parsedUserId))
        {
            return Result.Failure<GetMeResponse>(new("invalid_user", "Invalid user", ErrorType.Validation))
                .ToActionResult();
        }
        
        
        var result = await authService.GetMeAsync(parsedUserId);
        
        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToActionResult();
    }
    
    [HttpPost("create-role")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> CreateRole([FromBody] CreateRoleRequest request)
    {
        var result = await authService.CreateRoleAsync(request);
        
        return result.IsSuccess
            ? Created()
            : result.ToActionResult();
    }
    
    [HttpPost("add-user-to-role")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> AddUserToRole([FromBody] RoleRequest request)
    {
        var result = await authService.AddUserToRoleAsync(request);
        
        return result.IsSuccess
            ? Ok()
            : result.ToActionResult();
    }
    
    [HttpPost("remove-user-from-role")]
    [Authorize]
    public async Task<ActionResult> RemoveUserFromRole([FromBody] RoleRequest request)
    {
        var result = await authService.RemoveUserFromRoleAsync(request);
        
        return result.IsSuccess
            ? Ok()
            : result.ToActionResult();
    }
}