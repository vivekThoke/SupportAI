using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
