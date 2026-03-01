using Aero.Domain.Helpers;
using System;

namespace Aero.Domain.Entities;

public sealed class TransactionCode
{
      public string Name { get; set; } = string.Empty;
        public short Value { get; set; }
        public string Description { get; set; } = string.Empty;

      public TransactionCode() { }

      public TransactionCode(string name, short value, string description)
      {
            Name = ValidateRequiredString(name, nameof(name));
            Value = value;
            Description = ValidateRequiredString(description, nameof(description));
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
