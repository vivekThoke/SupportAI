using MediatR;
using Microsoft.AspNetCore.Mvc;
using SupportAI.Application.Auth.Commands;
using SupportAI.Application.Auth.DTOs;

namespace SupportAI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterTenantRequest request)
        {
            var command = new RegisterTenantCommand(
                request.TenantName,
                request.Email,
                request.Password
            );

            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var command = new LoginCommand(
                request.Email,
                request.Password
            );

            var result = await _mediator.Send(command);

            return Ok(result);
        }
    }
}
