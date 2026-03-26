using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
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

            var response = await _httpClient.PutAsJsonAsync("http://localhost:6333/collections/documents/points?wait=true", payload);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Qdrant error: {error}");
            }

            return id;
        }

        public async Task<List<string>> SearchAsync(
            List<float> embedding,
            Guid tenantId,
            int topK = 5)
        {
            var vector = embedding.Select(x => (double)x).ToList();

            var request = new
            {
                vector = vector,
                limit = topK,
                with_payload = true,
                filter = new
                {
                    must = new[]
                    {
                        new
                        {
                            key = "tenantId",
                            match = new
                            {
                                value = tenantId.ToString()
                            }
                        }
                    }
                }
            };

            var response = await _httpClient.PostAsJsonAsync(
                                        "http://localhost:6333/collections/documents/points/search",
                                        request);

            var json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(json);
            }

            var result = JsonDocument.Parse(json);

            var match = result.RootElement
                            .GetProperty("result")
                            .EnumerateArray();

            var contents = new List<string>();

            foreach (var item in match)
            {
                if (!item.TryGetProperty("payload", out var payload))
                    continue;

                if (!payload.TryGetProperty("content", out var contentElement))
                    continue;

                var content = contentElement.GetString();

                if (!string.IsNullOrEmpty(content))
                {
                    contents.Add(content);
                }
            }

            return contents;
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
