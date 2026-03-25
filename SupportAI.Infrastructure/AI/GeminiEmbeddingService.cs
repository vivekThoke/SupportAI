using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SupportAI.Application.Interfaces;
using SupportAI.Infrastructure.AI.Models;
using UglyToad.PdfPig.Graphics.Colors;

namespace SupportAI.Infrastructure.AI
{
    public class GeminiEmbeddingService : IEmbeddingService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public GeminiEmbeddingService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["Gemini:ApiKey"];
        }

        public async Task<List<float>> GenerateEmbeddingAsync(List<string> text)
        {
            var random = new Random();

            return Enumerable.Range(0, 768)
                             .Select(_ => {
                            // 1. Generate 0.0 to 1.0
                            // 2. Multiply by 2.0 (Range: 0.0 to 2.0)
                            // 3. Subtract 1.0 (Range: -1.0 to 1.0)
                            double shiftedValue = (random.NextDouble() * 2.0) - 1.0;

                            // 4. Round to 3 decimal places
                            return (float)Math.Round(shiftedValue, 3);
                        })
                        .ToList();
        }

        public async Task<List<float>> GenerateEmbeddingAsync(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentException("Text cannot be empty", nameof(text));

            var envKey = Environment.GetEnvironmentVariable("GEMINI_API_KEY");

            // Update model Id
            const string modelId = "gemini-embedding-001";

            var requestBody = new
            {
                model = $"models/{modelId}",
                content = new
                {
                    parts = new[] { new { text = text } }
                },
                outputDimensionality = 768,
            };

            var url = $"https://generativelanguage.googleapis.com/v1beta/models/{modelId}:embedContent?key={_apiKey}";

            var response = await _httpClient.PostAsJsonAsync(url, requestBody);

            var responseText = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Gemini Embedding Error: {responseText}");
            }
            

            var result = await response.Content.ReadFromJsonAsync<GeminiEmbeddingResponse>();

            var values = result?.embedding?.values;

            if (values == null || values.Count == 0)
                throw new Exception("Invalid Embedding response: No value returned");

            return values;
        }
    }
}
