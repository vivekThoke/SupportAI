using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Connections;
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
        private readonly IBackgroundJobQueue _backgroundJobQueue;

        public DocumentProcessor(AppDbContext appDbContext, TextExtractor textExtractor, TextChunker textChunker, IEmbeddingService embeddingService, IVectorDatabase vectorDatabase, IBackgroundJobQueue backgroundJobQueue)
        {
            _appDbContext = appDbContext;
            _textChunker = textChunker;
            _textExtractor = textExtractor;
            _embeddingService = embeddingService;
            _vectorDatabase = vectorDatabase;
            _backgroundJobQueue = backgroundJobQueue;
        }

        public async Task ProcessSingleAsync(Guid documentId)
        {
            var doc = await _appDbContext.Documents
                                    .FirstOrDefaultAsync(x => x.Id == documentId);

            if (doc == null) return;

            if (doc.Status == "Ready") return;

            try
            {
                if (!File.Exists(doc.FilePath))
                    throw new Exception("File not found");

                var text = _textExtractor.Extract(doc.FilePath);

                if (string.IsNullOrEmpty(text))
                    throw new Exception("Empty document content");

                var chunks = _textChunker.Chunk(text);

                var chunkEntities = new List<DocumentChunk>();

                foreach (var chunk in chunks)
                {
                    if (string.IsNullOrEmpty(chunk))
                        continue;

                    await Task.Delay(100);

                    //var embedding = await _embeddingService.GenerateEmbeddingAsync(chunk);

                    var embedding = await _embeddingService.GenerateEmbeddingAsync("Hello World");
                    Debug.WriteLine($"Embedding Size: {embedding.Count}");

                    if (embedding == null || embedding.Count == 0)
                        throw new Exception("Embedding Failed");

                    await _vectorDatabase.EnsureCollectionExists();

                    var vectorId = await _vectorDatabase.UpsertAsync(
                        embedding,
                        doc.TenantId,
                        doc.Id,
                        chunk
                    );

                    //var vectorId = await _vectorDatabase.UpsertAsync(
                    //        Enumerable.Repeat(0.5f, 768).ToList(),
                    //        Guid.NewGuid(),
                    //        Guid.NewGuid(),
                    //        "test content"
                    //);

                    chunkEntities.Add(new DocumentChunk(
                        doc.TenantId,
                        doc.Id,
                        chunk,
                        vectorId
                    ));
                }

                if (!chunkEntities.Any())
                    throw new Exception("No valid chunk generated");

                await _appDbContext.DocumentChunks.AddRangeAsync(chunkEntities);

                doc.MarkReady();

                await _appDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                doc.MarkFailed(ex.Message);

                await _appDbContext.SaveChangesAsync();

                //retry logic
                if (doc.CanRetry())
                {
                    await Task.Delay(2000);
                    _backgroundJobQueue.Enqueue(doc.Id);
                }
            }
        }
    }
}
