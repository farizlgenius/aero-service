using Aero.Domain.Helpers;
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

      public RefreshToken() { }

      public RefreshToken(string hashedToken, string userId, string username, string action, string? info, DateTime expireDate)
      {
          HashedToken = ValidateRequiredString(hashedToken, nameof(hashedToken));
          UserId = ValidateRequiredString(userId, nameof(userId));
          Username = ValidateRequiredString(username, nameof(username));
          Action = ValidateRequiredString(action, nameof(action));
          Info = info;
          ExpireDate = expireDate;
      }

      private static string ValidateRequiredString(string value, string field)
      {
          ArgumentException.ThrowIfNullOrWhiteSpace(value, field);
          var trimmed = value.Trim();
          if (!RegexHelper.IsValidName(trimmed) && !RegexHelper.IsValidOnlyCharAndDigit(trimmed.Replace("-", string.Empty).Replace("_", string.Empty)))
          {
              throw new ArgumentException($"{field} invalid.", field);
          }

          return value;
      }
}
