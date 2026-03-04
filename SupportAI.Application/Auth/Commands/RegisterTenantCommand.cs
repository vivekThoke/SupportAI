using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SupportAI.Application.Auth.DTOs;

namespace SupportAI.Application.Auth.Commands
{
    public record RegisterTenantCommand(
            string TenantName,
            string Email,
            string Password
        ) : IRequest<AuthResponse>;
}
