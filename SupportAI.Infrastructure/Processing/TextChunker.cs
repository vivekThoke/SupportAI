using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UglyToad.PdfPig.DocumentLayoutAnalysis;

namespace SupportAI.Infrastructure.Processing
{
    public class TextChunker
    {
        public List<string> Chunk(string text, int size = 500)
        {
            var chunks = new List<string>();

            for (int i = 0; i < text.Length; i += size)
            {
                chunks.Add(text.Substring(i, Math.Min(size, text.Length - i)));
            }

            return chunks;
        }

        public List<string> Chunk(string text)
        {
            const int chunkSize = 500;
            const int overleap = 100;

            var chunks = new List<string>();

            for (int i = 0; i < text.Length; i += (chunkSize - overleap))
            {
                var length = Math.Min(chunkSize, text.Length - i);
                var chunk = text.Substring(i, length);

                chunks.Add(chunk);
            }
            return chunks;
        }
    }
}
