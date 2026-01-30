using System;

namespace Aero.Domain.Entities;

public sealed class MonitorGroup : BaseEntity
{
      public string Name { get; set; } = string.Empty;
        public short nMpCount { get; set; }
        public List<MonitorGroupList> nMpList { get; set; } = new List<MonitorGroupList>();
}
