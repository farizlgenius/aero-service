using Aero.Domain.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aero.Domain.Entities
{
    public sealed class Company : BaseDomain
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set;  } = string.Empty;
        
        public Company(int Id,string name,string description,int location,bool status) : base(location,status)
        {
            SetId(Id);
            SetName(name);
            this.Description = description;
        }

        private void SetId(int id)
        {
            if (id <= 0) throw new ArgumentException("Id invalid.");
            Id = id;
        }

        private void SetName(string name)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            if (!RegexHelper.IsValidName(name)) throw new ArgumentException("Name invalid.");
            this.Name = name.Trim();
        }


    }
}
