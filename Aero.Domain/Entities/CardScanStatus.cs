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
}