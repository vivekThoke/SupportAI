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
    }
}
