using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SupportAI.Application.Interfaces;
using SupportAI.Domain.Entities;
using SupportAI.Infrastructure.Persistence;

namespace SupportAI.Infrastructure.Repositories
{
    public class TenantRepository : ITenantRepository
    {
        private readonly AppDbContext dbContext;

        public TenantRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddAsync(Tenant tenant)
        {
            await dbContext.Tenants.AddAsync(tenant);

            await dbContext.SaveChangesAsync();
        }

        public async Task<Guid?> GetTenantIdAsync(string tenantName)
        {
            return await dbContext.Tenants
                .Where(t => t.Name == tenantName)
                .Select(t => t.Id)
                .FirstOrDefaultAsync();
        }
    }
}
