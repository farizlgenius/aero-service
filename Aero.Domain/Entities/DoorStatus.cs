using Aero.Domain.Helpers;
using System;

namespace Aero.Domain.Entities;

public sealed class DoorStatus
{
      public string Mac {get; set;} = string.Empty;
      public short AcrId {get; set;}
      public string AcrMode {get; set;} = string.Empty;
      public string AccessPointStatus {get; set;} = string.Empty;

      public DoorStatus() { }

      public DoorStatus(string mac, short acrId, string acrMode, string accessPointStatus)
      {
            Mac = ValidateRequiredString(mac, nameof(mac));
            AcrId = acrId;
            AcrMode = ValidateRequiredString(acrMode, nameof(acrMode));
            AccessPointStatus = ValidateRequiredString(accessPointStatus, nameof(accessPointStatus));
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
