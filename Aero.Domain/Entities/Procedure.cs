using Aero.Domain.Helpers;
using System;

namespace Aero.Domain.Entities;

public sealed class Procedure : BaseDomain
{
    public int DeviceId { get; set; }
      public short DriverId {get; set;}
      public int TriggerId {get; set;}
      public string Name { get; set; } = string.Empty;
      public List<Action> Actions { get; set; } = new List<Action>();

      public Procedure() { }

      public Procedure(int deviceId, short driverId, int triggerId, string name, List<Action> actions)
      {
          DeviceId = deviceId;
          DriverId = driverId;
          TriggerId = triggerId;
          Name = ValidateRequiredString(name, nameof(name));
          Actions = actions ?? throw new ArgumentNullException(nameof(actions));
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
