using System;

namespace Aero.Domain.Entities;

public sealed class TimeZone : BaseDomain
{
      public int Id { get; private set; }
      public short DriverId { get; set; }
      public string Name { get; set; } = string.Empty;
      public short Mode { get; set; }
      public string ActiveTime { get; set; } = string.Empty;
      public string DeactiveTime { get; set; } = string.Empty;
      public List<Interval> Intervals { get; set; } = new List<Interval>();

      public TimeZone(short driver_id, string name, short mode, string active_time, string deactive_time, int location_id, bool status) : base(location_id, status)
      {
            this.DriverId = driver_id;
            this.Name = name;
            this.Mode = mode;
            this.ActiveTime = active_time;
            this.DeactiveTime = deactive_time;
      }

}
