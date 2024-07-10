namespace Application.Models.Auth;

public class AuthContext
{
    public Guid UserId { get; set; }

    public List<int> TenantIds { get; set; } = [];

    public List<Role> Roles { get; set; } = [];
    
    public bool IsSuperAdmin => Roles.Contains(Role.SuperAdmin);
}