using Aero.Domain.Helpers;
using System;

namespace Aero.Domain.Entities;

public sealed class CardFormat : BaseDomain
{
        public int Id { get; private set; }
    public short DriverId { get; private set; }
      public string Name { get; private set; } = string.Empty;
      public short Facility { get; private set; }
      public short Offset {get; private  set;}
      public short FunctionId {get; private set; }
      public short Flags {get; private  set;}
      public short Bits { get; private set; }
      public short PeLn { get; private  set; }
      public short PeLoc { get; private set; }
      public short PoLn { get; private  set; }
      public short PoLoc { get; private  set; }
      public short FcLn { get; private  set; }
      public short FcLoc { get; private  set; }
      public short ChLn { get; private set; }
      public short ChLoc { get; private set; }
      public short IcLn { get; private set; }
      public short IcLoc { get; private set; }

      public CardFormat(short driver,string name,short fac,short offset,short funcId,short flags,short bits,short peLn,short peLoc,short poLn,short poLoc,short fcLn,short fcLoc,short chLn,short chLoc,short icLn,short icLoc)
    {
        SetName(name);
        SetDriverId(driver);
        Facility = fac;
        Offset = offset;
        FunctionId = funcId;
        Flags = flags;
        Bits = bits;
        SetParityEven(peLn,peLoc);
        SetParityOdd(poLn,poLoc);
        SetFac(fcLn,fcLoc);
        SetCardNo(chLn,chLoc);
        SetIssueCode(icLn,icLoc);
    }

    private void SetDriverId(short driver) 
    {
        if (driver <= 0) throw new ArgumentException("Driver id invalid.");
        this.DriverId = driver;
    }

    private void SetName(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        if (!RegexHelper.IsValidName(name)) throw new ArgumentException("Name invalid.");
    }

    private void SetParityEven(short PeLn,short PeLoc)
    {
        if (PeLoc < 0 && PeLoc >= Bits) throw new ArgumentException("Event Parity invalid.");
        if (PeLn + PeLoc >= Bits) throw new ArgumentException("Event Parity invalid.");

        this.PeLoc = PeLoc;
        this.PeLn = PeLn;
    }

    private void SetParityOdd(short PoLn, short PoLoc)
    {
        if (PoLoc < 0 && PoLoc >= Bits) throw new ArgumentException("Odd Parity invalid.");
        if (PoLn + PoLoc >= Bits) throw new ArgumentException("Odd Parity invalid.");

        this.PoLoc = PoLoc;
        this.PoLn = PoLn;
    }

    private void SetFac(short FcLn, short FcLoc)
    {
        if (FcLoc < 0 && FcLoc >= Bits) throw new ArgumentException("Fac invalid.");
        if (FcLn + FcLoc >= Bits) throw new ArgumentException("Fac invalid.");

        this.FcLoc = FcLoc;
        this.FcLn = FcLn;
    }

    private void SetCardNo(short ChLn, short ChLoc)
    {
        if (ChLoc < 0 && ChLoc >= Bits) throw new ArgumentException("Card no invalid.");
        if (ChLn + ChLoc >= Bits) throw new ArgumentException("Card no invalid.");

        this.ChLoc = ChLoc;
        this.ChLn = ChLn;
    }

    private void SetIssueCode(short IcLn, short IcLoc)
    {
        if (IcLoc < 0 && IcLoc >= Bits) throw new ArgumentException("Issue code invalid.");
        if (IcLn + IcLoc >= Bits) throw new ArgumentException("Issue code invalid.");

        this.IcLoc = IcLoc;
        this.IcLn = IcLn;
    }



}
