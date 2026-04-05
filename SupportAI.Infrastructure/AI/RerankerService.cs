using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SupportAI.Application.Interfaces;

namespace SupportAI.Infrastructure.AI
{
    public class RerankerService
    {
        private readonly IChatService _chatService;

        public RerankerService(IChatService chatService)
        {
            _chatService = chatService;
        }

        public async Task<List<string>> RerankAsync(string question, List<string> chunks)
        {
            var joined = string.Join("\n\n", chunks);

            var prompt = $@"
                You are an AI that selects the most relevant information.
            
                Question: 
                {question}

                Chunks:
                {joined}

                Return ONLY the 5 most relevant chunks";


            var result = await _chatService.AskAsync(question, new List<string> { prompt });

            return result.Split("\n\n").Take(5).ToList();
        }
    }
}
