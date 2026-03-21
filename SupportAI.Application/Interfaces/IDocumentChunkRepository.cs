using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SupportAI.Domain.Entities;

namespace SupportAI.Application.Interfaces
{
    public interface IDocumentChunkRepository
    {
        Task AddRangeAsync(List<DocumentChunk> chunks);
    }
}
