using System;

namespace Aero.Domain.Entities;

public sealed class MonitorGroupList
{
    public int MonitorGroupId { get; set; } 
      public short PointType { get; set; }
        public string PointTypeDetail { get; set; } = string.Empty;
        public short PointNumber { get; set; }

    
}
