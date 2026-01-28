using System;

namespace Aero.Domain.Entities;

public sealed class CardFormat : NoMacBaseEntity
{
      public string Name { get; set; } = string.Empty;
      public short Facility { get; set; }
      public short Offset {get; set;}
      public short FunctionId {get; set;}
      public short Flags {get; set;}
      public short Bits { get; set; }
      public short PeLn { get; set; }
      public short PeLoc { get; set; }
      public short PoLn { get; set; }
      public short PoLoc { get; set; }
      public short FcLn { get; set; }
      public short FcLoc { get; set; }
      public short ChLn { get; set; }
      public short ChLoc { get; set; }
      public short IcLn { get; set; }
      public short IcLoc { get; set; }

}
