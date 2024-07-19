using Domain.Entities;
using Infra.Identity.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories;

internal sealed class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<User, Role, Guid>(options)
{
    public DbSet<Tenant> Tenants { get; set; }

    public DbSet<TagGroup> TagGroups { get; set; }

    public DbSet<Tag> Tags { get; set; }
}