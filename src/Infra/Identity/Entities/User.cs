using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infra.Identity.Entities;

public class User : IdentityUser<Guid>
{
    public List<Tenant> Tenants { get; set; } = [];

    public List<Role> Roles { get; set; } = [];
}

public class Role : IdentityRole<Guid>
{
    public List<User> Users { get; set; } = [];
}