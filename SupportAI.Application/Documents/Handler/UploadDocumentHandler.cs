using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SupportAI.Application.Documents.Commands;
using SupportAI.Application.Interfaces;
using SupportAI.Domain.Entities;

namespace SupportAI.Application.Documents.Handler
{
    public class UploadDocumentHandler : IRequestHandler<UploadDocumentCommand, Guid>
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IFileStorageService _fileStorageService;
        private readonly ITenantRepository _tenantRepository;
        private readonly IBackgroundJobQueue _queue;

        public UploadDocumentHandler(IDocumentRepository documentRepository, IFileStorageService fileStorageService, ITenantRepository tenantRepository, IBackgroundJobQueue queue)
        {
            _documentRepository = documentRepository;
            _fileStorageService = fileStorageService;
            _tenantRepository = tenantRepository;
            _queue = queue;
        }
            
        public async Task<Guid> Handle(
            UploadDocumentCommand command, 
            CancellationToken token)
        {
            var tenantId = await _tenantRepository.GetTenantIdAsync(command.TenantName);

            // Save file to disk
            var filePath = await _fileStorageService.SaveAsync(
                                        command.FileStream,
                                        command.FileName);

            // Create a new document entity
            var document = new Document(tenantId.Value,
                                command.FileName,
                                filePath);

            // To-Do: For now making document has mark ready.
            document.MarkProcessing();
           
            // Save to DB
            await _documentRepository.AddAsync(document);

            _queue.Enqueue(document.Id);

            return document.Id;
        }
    }
}
