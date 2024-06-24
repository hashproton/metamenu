using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories;

internal sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Tenant> Tenants { get; set; }

    public DbSet<TagGroup> TagGroups { get; set; }

    public DbSet<Tag> Tags { get; set; }
}