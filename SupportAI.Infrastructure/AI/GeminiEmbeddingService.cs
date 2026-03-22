using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SupportAI.Application.Interfaces;
using UglyToad.PdfPig.Graphics.Colors;

namespace SupportAI.Infrastructure.AI
{
    public class GeminiEmbeddingService : IEmbeddingService
    {
        public async Task<List<float>> GenerateEmbeddingAsync(List<string> text)
        {
            var random = new Random();

            return Enumerable.Range(0, 786)
                .Select(_ => (float)random.NextDouble())
                .ToList();
        }
    }
}
