using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aero.Infrastructure.Persistences.Entities
{
    public sealed class Position : BaseEntity
    {
        public string name { get; set; } = string.Empty;
        public string description { get; set;  } = string.Empty;
        public ICollection<User> users { get; set;}
        public Position(){}

        public Position(Aero.Domain.Entities.Position data) : base(data.LocationId)
        {
            this.name = data.Name;
            this.description = data.Description;
        }

        public void Update(Aero.Domain.Entities.Position data) 
        {
            this.name = data.Name;
            this.description = data.Description;
            this.updated_date = DateTime.UtcNow;
        }
    }
}

