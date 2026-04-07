using System;
using System.ComponentModel.DataAnnotations;

namespace Aero.Infrastructure.Persistences.Entities;

public sealed class LedConfig 
{
      [Key]
      public int id {get; set;}
      public int led_id {get; set;}
      public Led led {get; set;}
      public short r_led_id {get; set;}
      public short on_color {get; set;}
      public short off_color {get; set;}
      public short on_time {get; set;}
      public short off_time {get; set;}
      public short repeat_count {get; set;}
      public short beep_count {get; set;}

      public LedConfig(int id,int led_id,short r_led_id,short on_color,short off_color,short on_time,short off_time,short repeat_count,short beep_count)
      {
            this.id = id;
            this.led_id = led_id;
            this.r_led_id = r_led_id;
            this.on_color = on_color;
            this.off_color = off_color;
            this.on_time = on_time;
            this.off_time = off_time;
            this.repeat_count = repeat_count;
            this.beep_count = beep_count;
      }

      public LedConfig(Aero.Domain.Entities.LedConfig data)
      {
            this.led_id = data.LedId;
            this.r_led_id = data.RLedId;
            this.on_color = data.OnColor;
            this.off_color = data.OffColor;
            this.on_time = data.OnTime;
            this.off_time = data.OffTime;
            this.repeat_count = data.RepeatCount;
            this.beep_count = data.BeepCount;
      }

      public void Update(Aero.Domain.Entities.LedConfig data)
      {
            this.led_id = data.LedId;
            this.r_led_id = data.RLedId;
            this.on_color = data.OnColor;
            this.off_color = data.OffColor;
            this.on_time = data.OnTime;
            this.off_time = data.OffTime;
            this.repeat_count = data.RepeatCount;
            this.beep_count = data.BeepCount;
      }
}
