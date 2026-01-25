using System;
using Aero.Domain.Entities;

namespace Aero.Domain.Interfaces;

public interface ITzCommand
{
  bool ExtendedTimeZoneActSpecification(short ScpId, Timezone dto, List<Interval> intervals, int activeTime, int deactiveTime);
  bool TimeZoneControl(short ScpId,short Component,short Command);
}
