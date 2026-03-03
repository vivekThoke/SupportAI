using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using SupportAI.Domain.Common;
using SupportAI.Domain.Enum;

namespace SupportAI.Domain.Entities
{
    public class User : BaseEntity
    {
        public Guid TenantId { get; private set; }
        public string Email { get; private set; } = default!;
        public string PasswordHash { get; private set; } = default!;
        public UserRoles Role { get; private set; } = default!;

        private User() { }

        public User(Guid tenantId, string email, string passwordHash, UserRoles role)
        {
            if (tenantId == Guid.Empty)
                throw new ArgumentException("Tenant Id is required");

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email is required");

            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ArgumentException("Password is required");

            TenantId = tenantId;
            Email = email;
            PasswordHash = passwordHash;
            Role = role;
        }
    }
}
