using System.ComponentModel.DataAnnotations;
using Aero.Domain.Entities;
using Aero.Domain.Interface;

namespace Aero.Infrastructure.Persistences.Entities
{
    public sealed class Operator 
    {
        [Key]
        public int id { get; set; }
        public required string user_id { get; set; }
        public required string user_name { get; set; }
        public required string password { get; set; } 
        public string email { get; set; } = string.Empty;
        public string title { get; set; } = string.Empty;
        public string first_name { get; set; } = string.Empty;
        public string middle_name { get; set; } = string.Empty;
        public string last_name { get; set; } = string.Empty;
        public string phone { get; set; } = string.Empty;
        public string image { get; set; } = string.Empty;
        public short role_id { get; set; }
        public Role role { get; set; }
        public ICollection<OperatorLocation> operator_locations { get; set; }
        public bool is_active { get; set; } = true;
        public DateTime created_date { get; set;} = DateTime.UtcNow;
        public DateTime updated_date { get; set; } = DateTime.UtcNow;

        public Operator(){}

        public Operator(Aero.Domain.Entities.Operator data) 
        {
            this.user_id = data.UserId;
            this.user_name = data.Username;
            this.email = data.Email;
            this.title = data.Title;
            this.first_name = data.FirstName;
            this.middle_name = data.MiddleName;
            this.last_name = data.LastName;
            this.phone = data.Phone;
            this.image = data.Image;
            this.role_id = data.RoleId;
        }

        public void ToggleStatus(bool status)
        {
            this.is_active = status;
        }


    }
}
