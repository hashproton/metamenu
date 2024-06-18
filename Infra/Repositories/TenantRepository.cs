using Application.Repositories;
using Domain.Entities;
using Infra.Repositories.Common;

namespace Infra.Repositories;

public class TenantRepository(AppDbContext context) : GenericRepository<Tenant>(context), ITenantRepository;