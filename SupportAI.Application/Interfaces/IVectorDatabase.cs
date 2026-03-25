using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportAI.Application.Interfaces
{
    public interface IVectorDatabase
    {
        Task<string> UpsertAsync(
            List<float> embeddings,
            Guid tenantId,
            Guid documentId,
            string content
            );

        Task EnsureCollectionExists();

        Task<List<string>> SearchAsync(
            List<float> embedding,
            Guid tenantId,
            int topK = 5);
    }
}
