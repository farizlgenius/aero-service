using System;

namespace Aero.Domain.Entities;

public sealed class RefreshToken
{

      public string HashedToken { get; set; } = default!; // store hashed token
      public string UserId { get; set; } = default!;
      public string Username { get; set; } = default!;
      public string Action { get; set; } = default!; // "create", "rotate", "revoke"
      public string? Info { get; set; } // optional JSON for ip/user-agent
      public DateTime ExpireDate { get; set; } = DateTime.UtcNow;

}
