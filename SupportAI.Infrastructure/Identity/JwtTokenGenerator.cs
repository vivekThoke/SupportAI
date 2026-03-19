using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using SupportAI.Application.Interfaces;
using SupportAI.Domain.Entities;


namespace SupportAI.Infrastructure.Identity
{

    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly string _secretKey = "SUPER_SECRET_KEY_123_AROUND_THE_WORLD_TRAVEL_WITH_ONE_PURPOSE_TO_MAKE_NEW_THINGS_THAT_MAKE_THIS_THINGS_COME_TRUE";

        public string GenerateToken(User user)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_secretKey));

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim("tenantId", user.TenantId.ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var token = new JwtSecurityToken
            (
                claims: claims,
                expires: DateTime.UtcNow.AddHours(6),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
