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
        public int RetryCount { get; private set; }
        public string? ErrorMessage { get; private set; }
        public DateTime? LastProcessedAt { get; private set; }

        private Document() { }

        public Document(Guid tenantId, string fileName, string filePath)
        {
            TenantId = tenantId;
            FileName = fileName;
            FilePath = filePath;
        }

        public void MarkReady()
        {
            Status = "Ready";
            ErrorMessage = null;
            LastProcessedAt = DateTime.UtcNow;
        }
        public void MarkFailed(string error)
        {
            Status = "Failed";
            ErrorMessage = error;
            RetryCount++;
            LastProcessedAt = DateTime.UtcNow;
        }
        public void MarkProcessing()
        {
            Status = "Processing";
            ErrorMessage = null;
            LastProcessedAt = DateTime.UtcNow;
        }

        public bool CanRetry() => RetryCount < 3;
    }
}
