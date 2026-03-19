using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportAI.Application.Auth.DTOs
{
    public record LoginRequest(string Email, String Password);
}
