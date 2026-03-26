using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SupportAI.Application.Interfaces;

namespace SupportAI.Application.Chat.Queries
{
    public class AskQuestionHandler : IRequestHandler<AskQuestionQuery, string>
    {
        private readonly IEmbeddingService _embeddingService;
        private readonly IVectorDatabase _vectorDatabase;
        private readonly IChatService _chatService;
        private readonly ITenantRepository _tenantProvider;

        public AskQuestionHandler(IEmbeddingService embeddingService, IVectorDatabase vectorDatabase, IChatService chatService, ITenantRepository tenantProvider)
        {
            _embeddingService = embeddingService;
            _vectorDatabase = vectorDatabase;
            _chatService = chatService;
            _tenantProvider = tenantProvider;
        }

        public async Task<string> Handle(AskQuestionQuery request, CancellationToken token)
        {
            var tenantId = await _tenantProvider.GetTenantIdAsync(request.tenantName);

            var embedding = await _embeddingService.GenerateEmbeddingAsync(request.question);

            var chunks = await _vectorDatabase.SearchAsync(embedding, tenantId, 5);

            if (!chunks.Any())
                return "No relevant information found";

            var answer = await _chatService.AskAsync(request.question, chunks);

            return answer;
        }
    }
}
