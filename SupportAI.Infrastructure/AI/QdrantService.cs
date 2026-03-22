using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using SupportAI.Application.Interfaces;

namespace SupportAI.Infrastructure.AI
{
    public class QdrantService : IVectorDatabase
    {
        private readonly HttpClient _httpClient;

        public QdrantService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> UpsertAsync(
            List<float> embeddings,
            Guid tenantId,
            Guid documentId,
            string content
            )
        {
            var id = Guid.NewGuid().ToString();

            var payload = new
            {
                Points = new[]
                {
                    new
                    {
                        id = id,
                        vector = embeddings,
                        payload = new
                        {
                            tenantId,
                            documentId,
                            content
                        }
                    }
                }
            };

            await _httpClient.PutAsJsonAsync("http://localhost:6333/collections/documents/points", payload);

            return id;
        }
    }
}
