using Aero.Domain.Helpers;
using System;

namespace Aero.Domain.Entities;

public sealed class Password
{
      public string Username { get; set; } = string.Empty;
      public string Old { get; set; } = string.Empty;
      public string New { get; set; } = string.Empty;
      public string Con { get; set; } = string.Empty;

      public Password() { }

      public Password(string username, string oldPassword, string newPassword, string confirmPassword)
      {
            Username = ValidateRequiredString(username, nameof(username));
            Old = ValidateRequiredString(oldPassword, nameof(oldPassword));
            New = ValidateRequiredString(newPassword, nameof(newPassword));
            Con = ValidateRequiredString(confirmPassword, nameof(confirmPassword));
      }

      private static string ValidateRequiredString(string value, string field)
      {
            ArgumentException.ThrowIfNullOrWhiteSpace(value, field);
            var trimmed = value.Trim();
            var sanitized = trimmed.Replace("-", string.Empty).Replace("_", string.Empty);
            if (!RegexHelper.IsValidName(trimmed) && !RegexHelper.IsValidOnlyCharAndDigit(sanitized))
            {
                  throw new ArgumentException($"{field} invalid.", field);
            }

            return value;
      }
}
