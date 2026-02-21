using System.ComponentModel.DataAnnotations;
using Aero.Domain.Interface;

namespace Aero.Infrastructure.Persistences.Entities
{
    public sealed class RefreshToken : IDatetime
    {
        [Key]
        public int id { get; set; }
        public Guid uuid { get; set; } = Guid.NewGuid();
        public string hashed_token { get; set; } = default!; // store hashed token
        public string user_id { get; set; } = default!;
        public string user_name { get; set; } = default!;
        public string action { get; set; } = default!; // "create", "rotate", "revoke"
        public string? info { get; set; } // optional JSON for ip/user-agent
        public DateTime expire_date { get; set; } = DateTime.UtcNow;
        public DateTime created_date { get; set; } = DateTime.UtcNow;
        public DateTime updated_date { get; set; } = DateTime.UtcNow;


    }
}
