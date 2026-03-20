using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace SupportAI.Application.Documents.Commands
{
    public record UploadDocumentCommand(
        Stream FileStream,
        string FileName,
        string TenantName) : IRequest<Guid>;
}
