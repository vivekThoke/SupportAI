using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SupportAI.Domain.Common;

namespace SupportAI.Domain.Entities
{
    public class DocumentChunk : BaseEntity
    {
        public Guid TenantId { get; private set; }
        public Guid DocumentId { get; private set; }
        public string Content { get; private set; }
        public string VectorId { get; private set; }

        private DocumentChunk() { }

        public DocumentChunk(Guid tenantId, Guid documentId, string content, string vectorId)
        {
            TenantId = tenantId;
            DocumentId = documentId;
            Content = content;
            VectorId = vectorId;
        }
    }
}
