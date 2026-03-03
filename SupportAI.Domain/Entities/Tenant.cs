using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SupportAI.Domain.Common;

namespace SupportAI.Domain.Entities
{
    public class Tenant : BaseEntity
    {
        public string Name { get; private set; } = default!;

        private readonly List<User> users = new();
        public IReadOnlyCollection<User> Users => users.AsReadOnly();

        private Tenant() { }

        public Tenant(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Tenant cannot be empty");

            Name = name;
        }
    }
}
