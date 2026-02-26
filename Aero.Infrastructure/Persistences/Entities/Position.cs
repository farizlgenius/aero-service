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
        public int department_id { get; set; }
        public Department department { get; set; }
        public Position(Aero.Domain.Entities.Position data) : base(data.LocationId)
        {
            this.name = data.Name;
            this.description = data.Description;
            this.department_id = data.DepartmentId;
        }

        public void Update(Aero.Domain.Entities.Position data) 
        {
            this.name = data.Name;
            this.description = data.Description;
            this.department_id = data.DepartmentId;
        }
    }
}
