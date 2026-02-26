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
  public string Mac { get; set; } = string.Empty;
  public Device Device { get; set; }
  public Procedure Procedure { get; set; } = new Procedure();
}
