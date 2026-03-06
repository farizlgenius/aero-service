using Aero.Domain.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aero.Domain.Entities
{
    public sealed class Position : BaseDomain
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int DepartmentId {get;set;}

        public Position(string name, string description,int departmentId,int location, bool status) : base(location, status)
        {
            SetName(name);
            this.Description = description;
            SetDepartmentId(departmentId);
        }

        private void SetName(string name)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            if (!RegexHelper.IsValidName(name)) throw new ArgumentException("Name invalid.");
            this.Name = name.Trim();
        }

        private void SetDepartmentId(int department)
        {
            if(department <= 0) throw new ArgumentException("Deparment invalid.");
            DepartmentId = department;
        }
    }
}
