using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportAI.Application.Documents.DTOs
{
    public record UploadRequest(Stream File, string TenantName);
}
