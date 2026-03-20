using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using SupportAI.Application.Documents.Commands;
using SupportAI.Application.Documents.DTOs;

namespace SupportAI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DocumentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is required");

            var tenantName = Request.Headers["X-Tenant-Name"].FirstOrDefault();

            if (string.IsNullOrEmpty(tenantName))
                return BadRequest("Tenant header is missing");

            using var stream = file.OpenReadStream();

            var command = new UploadDocumentCommand(
                                    stream,
                                    file.FileName,
                                    tenantName);

            var documentId = await _mediator.Send(command);

            return Ok(new { documentId });
        }
    }
}
