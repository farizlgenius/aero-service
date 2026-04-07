using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aero.Infrastructure.Persistences.Entities
{
    public sealed class Department : BaseEntity
    {
        public string name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public ICollection<User> users { get; set; }
        public Department(){}

        public Department(string name,string description,int location_id) : base(location_id)
        {
            this.name = name;
            this.description = description;
        }

        public Department(Aero.Domain.Entities.Department data) : base(data.LocationId)
        {
            this.name = data.Name;
            this.description = data.Description;
        }

        public void Update(Aero.Domain.Entities.Department data)
        {
            this.name = data.Name;
            this.description = data.Description;
            this.updated_date = DateTime.UtcNow;
        }
    }
}

