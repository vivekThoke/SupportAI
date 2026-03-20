using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SupportAI.Domain.Common;

namespace SupportAI.Domain.Entities
{
    public class Document : BaseEntity
    {
        public Guid TenantId { get; private set; }
        public string FileName { get; private set; }
        public string FilePath { get; private set; }
        public string Status { get; private set; }

        private Document() { }

        public Document(Guid tenantId, string fileName, string filePath)
        {
            TenantId = tenantId;
            FileName = fileName;
            FilePath = filePath;
        }

        public void MarkReady() => Status = "Ready";
        public void MarkFailed() => Status = "Failed";
    }
}
