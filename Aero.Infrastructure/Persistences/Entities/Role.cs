using System.ComponentModel.DataAnnotations;
using Aero.Domain.Interface;

namespace Aero.Infrastructure.Persistences.Entities
{
    public sealed class Role : BaseEntity
    {
        public string name { get; set; } = string.Empty;
        public ICollection<Operator> operators { get; set; }
        public ICollection<Permission> permissions {get; set;}
    
        public Role()
        {
            operators = new List<Operator>();
            permissions = new List<Permission>();
        }

        public Role(Aero.Domain.Entities.Role role)
        {
            this.name = role.Name;
            operators = new List<Operator>();
            permissions = new List<Permission>();
        }

        public void Update(Aero.Domain.Entities.Role role)
        {
            this.name = role.Name;
        }


    }
}
