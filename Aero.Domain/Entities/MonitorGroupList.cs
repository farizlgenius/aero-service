using System;

namespace Aero.Domain.Entities;

public sealed class MonitorGroupList
{
      public short PointType { get; set; }
        public string PointTypeDesc { get; set; } = string.Empty;
        public short PointNumber { get; set; }
}
