using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SupportAI.Application.Interfaces;
using SupportAI.Domain.Entities;
using SupportAI.Infrastructure.Persistence;

namespace SupportAI.Infrastructure.Processing
{
    public class DocumentProcessor
    {
        private readonly AppDbContext _appDbContext;
        private readonly TextExtractor _textExtractor;
        private readonly TextChunker _textChunker;
        private readonly IEmbeddingService _embeddingService;
        private readonly IVectorDatabase _vectorDatabase;

        public DocumentProcessor(AppDbContext appDbContext, TextExtractor textExtractor, TextChunker textChunker, IEmbeddingService embeddingService, IVectorDatabase vectorDatabase)
        {
            _appDbContext = appDbContext;
            _textChunker = textChunker;
            _textExtractor = textExtractor;
            _embeddingService = embeddingService;
            _vectorDatabase = vectorDatabase;
        }

        public async Task ProcessAsync()
        {
            var documents = await _appDbContext.Documents
                        .Where(x => x.Status == "Processing")
                        .ToListAsync();

            foreach(var doc in documents)
            {
                try
                {
                    var text = _textExtractor.Extract(doc.FilePath);
                    var chunks = _textChunker.Chunk(text);

                    var embeddings = await _embeddingService.GenerateEmbeddingAsync(chunks);

                     var vectorId = await _vectorDatabase.UpsertAsync(
                         embeddings, 
                         doc.TenantId, 
                         doc.Id, 
                         chunks.ToString()
                     );

                    var chunkEntities = chunks.Select(c =>
                            new DocumentChunk(
                                doc.TenantId,
                                doc.Id,
                                c,
                                vectorId
                                )).ToList();

                    await _appDbContext.DocumentChunks.AddRangeAsync(chunkEntities);

                    doc.MarkReady();

                    await _appDbContext.SaveChangesAsync();
                }
                catch
                {
                    doc.MarkFailed();
                    await _appDbContext.SaveChangesAsync();
                }
            }
        }
    }
}
