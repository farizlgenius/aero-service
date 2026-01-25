using System;

namespace Aero.Domain.Entities;

public sealed class Timezone : NoMacBaseEntity
{
      public string Name { get; set; } = string.Empty;
      public short Mode { get; set; }
      public string ActiveTime { get; set; } = string.Empty;
      public string DeactiveTime { get; set; } = string.Empty;
      public List<Interval>? Intervals { get; set; }

}
