using MediatR;
using Microsoft.AspNetCore.Mvc;
using SupportAI.Application.Chat.Queries;

namespace SupportAI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : Controller
    {
        private readonly IMediator _mediator;
        
        public ChatController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("ask")]
        public async Task<IActionResult> Ask([FromBody] string question)
        {
            var tenantName = Request.Headers["X-Tenant-Name"].FirstOrDefault();

            if (string.IsNullOrEmpty(tenantName))
                return BadRequest("Tenant header is missing");

            var result = await _mediator.Send(new AskQuestionQuery(question, tenantName));

            return Ok(new { answer = result });
        }
    }
}
