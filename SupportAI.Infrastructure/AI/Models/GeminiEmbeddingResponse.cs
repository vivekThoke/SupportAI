using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportAI.Infrastructure.AI.Models
{
    public class GeminiEmbeddingResponse
    {
        public Embedding? embedding { get; set; }
    }

    public class Embedding
    {
        public List<float>? values { get; set; }
    }
}
