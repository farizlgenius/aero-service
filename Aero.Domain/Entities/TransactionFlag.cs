using Aero.Domain.Helpers;
using System;

namespace Aero.Domain.Entities;

public sealed class TransactionFlag
{
       public string Topic { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public TransactionFlag() { }

        public TransactionFlag(string topic, string name, string description)
        {
            Topic = ValidateRequiredString(topic, nameof(topic));
            Name = ValidateRequiredString(name, nameof(name));
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
