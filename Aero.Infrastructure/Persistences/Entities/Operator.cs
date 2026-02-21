using System.ComponentModel.DataAnnotations;
using Aero.Domain.Entities;
using Aero.Domain.Interface;

namespace Aero.Infrastructure.Persistences.Entities
{
    public sealed class Operator : BaseEntity
    {
        public required string user_id { get; set; }
        public required string user_name { get; set; }
        public required string password { get; set; } 
        public string email { get; set; } = string.Empty;
        public string title { get; set; } = string.Empty;
        public string first_name { get; set; } = string.Empty;
        public string middle_name { get; set; } = string.Empty;
        public string last_name { get; set; } = string.Empty;
        public string phone { get; set; } = string.Empty;
        public string image_path { get; set; } = string.Empty;
        public short role_id { get; set; }
        public Role role { get; set; }
        public ICollection<OperatorLocation> operator_locations { get; set; }


    }
}
