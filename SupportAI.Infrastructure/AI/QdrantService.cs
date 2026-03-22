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
                points = new[]
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

            var response = await _httpClient.PutAsJsonAsync("http://localhost:6333/collections/documents/points", payload);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Qdrant error: {error}");
            }

            return id;
        }

        public async Task EnsureCollectionExists()
        {
            var response = await _httpClient.GetAsync("http://localhost:6333/collections/documents");

            if (response.IsSuccessStatusCode)
                return;

            var payload =new
            {
                vector = new
                {
                    size = 786,
                    distance = "Cosine"
                }
            };

            await _httpClient.PostAsJsonAsync("http://localhost:6333/collections/documents", payload);
        }
    }
}
