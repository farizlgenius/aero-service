using Aero.Domain.Helpers;
using System;

namespace Aero.Domain.Entities;

public sealed class Trigger : BaseDomain
{
  public short DriverId { get; set; }
  public string Name { get; set; } = string.Empty;
  public short Command { get; set; }
  public short ProcedureId { get; set; }
  public short SourceType { get; set; }
  public short SourceNumber { get; set; }
  public short TranType { get; set; }
  public List<TransactionCode> CodeMap { get; set; } = new List<TransactionCode>();
  public short TimeZone { get; set; }
  public int DeviceId { get; set; } 
  public Device Device { get; set; } = new Device();
  public Procedure Procedure { get; set; } = new Procedure();

  public Trigger() { }

  public Trigger(short driverId, string name, short command, short procedureId, short sourceType, short sourceNumber, short tranType, List<TransactionCode> codeMap, short timeZone, int deviceId, Device device, Procedure procedure)
  {
      DriverId = driverId;
      Name = ValidateRequiredString(name, nameof(name));
      Command = command;
      ProcedureId = procedureId;
      SourceType = sourceType;
      SourceNumber = sourceNumber;
      TranType = tranType;
      CodeMap = codeMap ?? throw new ArgumentNullException(nameof(codeMap));
      TimeZone = timeZone;
      DeviceId = deviceId;
      Device = device ?? throw new ArgumentNullException(nameof(device));
      Procedure = procedure ?? throw new ArgumentNullException(nameof(procedure));
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
