using System.ComponentModel.DataAnnotations;
using Aero.Domain.Interface;

namespace Aero.Infrastructure.Data.Entities
{
    public sealed class Operator : IComponentId,IDatetime
    {
        [Key]
        public int id { get; set; }
        public short component_id { get; set; }
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
        public bool is_active { get; set; } = true;
        public DateTime created_date { get; set; }
        public DateTime updated_date { get; set; }

    }
}
