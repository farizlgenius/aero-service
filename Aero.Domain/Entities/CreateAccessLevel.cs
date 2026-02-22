using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aero.Domain.Entities
{
    public sealed class CreateAccessLevel : BaseDomain
    {
        public string Name { get; private set; } = string.Empty;
        public List<AccessLevelComponent> Components { get; private set; } = new List<AccessLevelComponent>();

        public CreateAccessLevel() { }

        public CreateAccessLevel(string name, List<AccessLevelComponent> components, int location) : base(location)
        {
            SetName(name);
            SetComponents(components);
        }


        private void SetName(string name)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            this.Name = name;
        }
        private void SetComponents(List<AccessLevelComponent> components)
        {
            this.Components = components;
        }
    }
}
