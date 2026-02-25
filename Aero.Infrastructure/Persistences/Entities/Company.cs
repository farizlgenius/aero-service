using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aero.Infrastructure.Persistences.Entities
{
    public sealed class Company : BaseEntity
    {
        public string name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public ICollection<Department> departments { get; set; }

        public Company(string name,string description,int location) : base(location) 
        {
            this.name = name;
            this.description = description;

        }

        public void Update(Aero.Domain.Entities.Company data)
        {
            this.name = data.Name;
            this.description = data.Description;
        }

        
    }
}
