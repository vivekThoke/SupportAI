using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportAI.Application.Interfaces
{
    public interface IRerankService
    {
        Task<List<string>> RerankAsync(string question, List<string> chunks);
    }
}
