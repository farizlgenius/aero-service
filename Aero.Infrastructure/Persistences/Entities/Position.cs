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
        public Position(string name,string description,int departmentId,int location) : base(location)
        {
            this.name = name;
            this.description = description;
            this.department_id = departmentId;
        }
    }
}
