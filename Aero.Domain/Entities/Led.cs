using System;

namespace Aero.Domain.Entities;

public sealed class Led
{
      public int Id {get; private set;}
      public short LedMode {get; private set;}
      public List<LedConfig> Config {get; private set;}

      public Led(int Id,short LedMode,List<LedConfig> Config)
      {
            this.Id = Id;
            this.LedMode = LedMode;
            this.Config = Config;
      }

}
