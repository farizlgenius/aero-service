using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class RefreshToken
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        // The token string (cryptographically random, base64)
        public string Token { get; set; } = default!;

        // Associated user identifier (username or user id)
        public string UserName { get; set; } = default!;

        // Optional link to the JWT id (jti) that this refresh token was issued with
        public string? JwtId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? CreatedByIp { get; set; }

        public DateTime ExpiresAt { get; set; }
        public bool IsRevoked { get; set; } = false;
        public DateTime? RevokedAt { get; set; }
        public string? ReplacedByToken { get; set; } // for rotation link

        // Convenience:
        public bool IsActive => !IsRevoked && DateTime.UtcNow < ExpiresAt;
    }
}
