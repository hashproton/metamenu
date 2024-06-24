using Application.Models.Auth;

namespace Application.Services.AuthService.Responses;

public class GetMeResponse
{
    public Guid Id { get; set; }
    public string Email { get; set; } = default!;

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public List<Role> Roles { get; set; } = [];

    public List<int> TenantIds { get; set; } = [];
}