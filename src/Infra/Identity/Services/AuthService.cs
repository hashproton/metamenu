using Application.Errors.Common;
using Infra.Identity.Entities;
using Infra.Identity.Services.Requests;
using Microsoft.AspNetCore.Identity;

namespace Infra.Identity.Services;

public class GetMeResponse
{
    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;
    
    public List<string> Roles { get; set; } = null!;
}

public class RoleRequest
{
    public Guid UserId { get; set; }
    
    public string Role { get; set; } = null!;
}

public class CreateRoleRequest
{
    public string Name { get; set; } = null!;
}

public interface IAuthService
{
    Task<Result<JwtAccessToken>> LoginAsync(LoginRequest request);

    Task<Result> RegisterAsync(RegisterRequest request);
    
    Task<Result<GetMeResponse>> GetMeAsync(Guid userId);
    
    Task<Result> AddUserToRoleAsync(RoleRequest request);
    
    Task<Result> RemoveUserFromRoleAsync(RoleRequest request);
    
    Task<Result> CreateRoleAsync(CreateRoleRequest request);
}


public class AuthService(
    UserManager<User> userManager,
    SignInManager<User> signInManager,
    RoleManager<Role> roleManager,
    IJwtService jwtService) : IAuthService
{
    public async Task<Result<JwtAccessToken>> LoginAsync(LoginRequest request)
    {
        var user = await userManager.FindByEmailAsync(request.EmailOrUsername);
        if (user is null)
        {
            return Result.Failure<JwtAccessToken>(new("invalid_user", "Invalid email or password", ErrorType.Validation));
        }

        // if (user.EmailConfirmed)
        // {
        //     return Result.Failure<JwtAccessToken>(new("email_not_confirmed", "Email not confirmed", ErrorType.Validation));
        // }
        
        var result = await signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (!result.Succeeded)
        {
            return Result.Failure<JwtAccessToken>(new("invalid_user", "Invalid email or password", ErrorType.Validation));
        }
        
        var token = await jwtService.GenerateAsync(user);
        
        return Result.Success(token);
    }

    public async Task<Result> RegisterAsync(RegisterRequest request)
    {
        var user = new User
        {
            Email = request.Email,
            UserName = request.Username
        };
        
        var result = await userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(x => new Error(x.Code, x.Description, ErrorType.Validation));
            
            return Result.Failure(errors);
        }

        return Result.Success();
    }

    public async Task<Result<GetMeResponse>> GetMeAsync(Guid userId)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user is null)
        {
            return Result.Failure<GetMeResponse>(new("user_not_found", "User not found", ErrorType.Validation));
        }
        
        var roles = await userManager.GetRolesAsync(user);
        
        return Result.Success(new GetMeResponse
        {
            Username = user.UserName,
            Email = user.Email,
            Roles = roles.ToList()
        });
    }
    
    public async Task<Result> AddUserToRoleAsync(RoleRequest request)
    {
        var user = await userManager.FindByIdAsync(request.UserId.ToString());
        if (user is null)
        {
            return Result.Failure(new Error("user_not_found", "User not found", ErrorType.Validation));
        }
        
        var result = await userManager.AddToRoleAsync(user, request.Role);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(x => new Error(x.Code, x.Description, ErrorType.Validation));
            
            return Result.Failure(errors);
        }

        return Result.Success();
    }
    
    public async Task<Result> RemoveUserFromRoleAsync(RoleRequest request)
    {
        var user = await userManager.FindByIdAsync(request.UserId.ToString());
        if (user is null)
        {
            return Result.Failure(new Error("user_not_found", "User not found", ErrorType.Validation));
        }
        
        var result = await userManager.RemoveFromRoleAsync(user, request.Role);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(x => new Error(x.Code, x.Description, ErrorType.Validation));
            
            return Result.Failure(errors);
        }

        return Result.Success();
    }

    public async Task<Result> CreateRoleAsync(CreateRoleRequest request)
    {
        var roleExists = await roleManager.RoleExistsAsync(request.Name);
        if (roleExists)
        {
            return Result.Failure(new Error("role_exists", "Role already exists", ErrorType.Conflict));
        }
        
        var role = new Role
        {
            Name = request.Name
        };
        
        var result = await roleManager.CreateAsync(role);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(x => new Error(x.Code, x.Description, ErrorType.Validation));
            
            return Result.Failure(errors);
        }
        
        return Result.Success();
    }
}