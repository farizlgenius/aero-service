using System;

namespace Aero.Domain.Entities;

public sealed class LedConfig 
{
      public int Id {get; private set;}
      public int LedId {get; private set;}
      public short RLedId {get; private set;}
      public short OnColor {get; private set;}
      public short OffColor {get; private set;}
      public short OnTime {get; private set;}
      public short OffTime {get; private set;}
      public short RepeatCount {get; private set;}
      public short BeepCount {get; private set;}

      public LedConfig(int id,int LedId,short RLedId,short OnColor,short OffColor,short Ontime,short OffTime,short RepeatCount,short BeepCount) 
      {
            this.Id = id;
            this.LedId = LedId;
            this.RLedId = RLedId;
            this.OnColor = OnColor;
            this.OffColor = OffColor;
            this.OnTime = Ontime;
            this.OffTime = OffTime;
            this.RepeatCount = RepeatCount;
            this.BeepCount = BeepCount;
      }

}
