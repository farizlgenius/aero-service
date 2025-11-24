using HIDAeroService.Entity.Interface;
using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class RefreshTokenAudit : IDatetime
    {
        [Key]
        public int Id { get; set; }
        public Guid Uuid { get; set; } = Guid.NewGuid();
        public string HashedToken { get; set; } = default!; // store hashed token
        public string UserId { get; set; } = default!;
        public string Username { get; set; } = default!;
        public string Action { get; set; } = default!; // "create", "rotate", "revoke"
        public string? Info { get; set; } // optional JSON for ip/user-agent
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;


    }
}
