using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SupportAI.Application.Interfaces;

namespace SupportAI.Infrastructure.Security
{
    public class PasswordHasher : IPasswordHasher
    {
        public string Hash(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool Verify(string password, string passWordHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, passWordHash);
        }
    }
}
