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
        public int company_id { get; set; }
        public Company company { get; set; }
        public ICollection<Position> positions { get; set; }
        public Department(string name,string description,int company_id,int location_id) : base(location_id)
        {
            this.name = name;
            this.description = description;
            this.company_id = company_id;
        }

        public void Update(Aero.Domain.Entities.Department data)
        {
            this.name = data.Name;
            this.description = data.Description;
            this.company_id = data.Company.Id;
        }
    }
}
