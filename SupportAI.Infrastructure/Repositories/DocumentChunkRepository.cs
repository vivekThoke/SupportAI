using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SupportAI.Application.Interfaces;
using SupportAI.Domain.Entities;
using SupportAI.Infrastructure.Persistence;

namespace SupportAI.Infrastructure.Repositories
{
    public class DocumentChunkRepository : IDocumentChunkRepository
    {
        private readonly AppDbContext _dbContext;

        public DocumentChunkRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddRangeAsync(List<DocumentChunk> chunks)
        {
            await _dbContext.Set<DocumentChunk>().AddRangeAsync(chunks);

            await _dbContext.SaveChangesAsync();
        }
    }
}
