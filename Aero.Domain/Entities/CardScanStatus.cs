using Aero.Domain.Helpers;
using System;

namespace Aero.Domain.Entities;


public sealed class CardScanStatus
{
      public string Mac { get; set; } = string.Empty;
      public int FormatNumber { get; set; }
      public int Fac { get; set; }
      public double CardId { get; set; }
      public int Issue { get; set; }
      public short Floor { get; set; }

      public CardScanStatus() { }

      public CardScanStatus(string mac, int formatNumber, int fac, double cardId, int issue, short floor)
      {
            Mac = ValidateRequiredString(mac, nameof(mac));
            FormatNumber = formatNumber;
            Fac = fac;
            CardId = cardId;
            Issue = issue;
            Floor = floor;
      }

      private static string ValidateRequiredString(string value, string field)
      {
            ArgumentException.ThrowIfNullOrWhiteSpace(value, field);
            var trimmed = value.Trim();
            var sanitized = trimmed.Replace("-", string.Empty).Replace("_", string.Empty).Replace(".", string.Empty).Replace(":", string.Empty).Replace("/", string.Empty);
            if (!RegexHelper.IsValidName(trimmed) && !RegexHelper.IsValidOnlyCharAndDigit(sanitized))
            {
                  throw new ArgumentException($"{field} invalid.", field);
            }

            return value;
      }
}
