using System;

namespace Aero.Domain.Entities;

public sealed class MonitorGroup : BaseDomain
{
      public string Name { get; set; } = string.Empty;
        public short nMpCount { get; set; }
        public List<MonitorGroupList> nMpList { get; set; } = new List<MonitorGroupList>();
}
