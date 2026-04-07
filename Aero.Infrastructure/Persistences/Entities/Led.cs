using System;

namespace Aero.Infrastructure.Persistences.Entities;

public sealed class Led : BaseEntity
{
      public short led_mode {get; set;}
      public ICollection<LedConfig> config {get; set;}


      public void Update(Aero.Domain.Entities.Led data)
      {
            this.led_mode = data.LedMode;
            this.config = data.Config.Select(x => new LedConfig(x.Id,x.LedId,x.RLedId,x.OnColor,x.OffColor,x.OnTime,x.OffTime,x.RepeatCount,x.BeepCount)).ToList();
            this.updated_date = DateTime.UtcNow;
      }

}
