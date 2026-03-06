using Aero.Domain.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aero.Domain.Entities
{
    public sealed class Department : BaseDomain
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Company Company { get; set; }

        public Department(string name, string description, int location, bool status) : base(location, status)
        {
            SetName(name);
            this.Description = description;
        }

        private void SetName(string name)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            if (!RegexHelper.IsValidName(name)) throw new ArgumentException("Name invalid.");
            this.Name = name.Trim();
        }

    }
}
