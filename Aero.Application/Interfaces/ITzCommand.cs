using System;
using Aero.Application.DTOs;

namespace Aero.Application.Interfaces;

public interface ITzCommand
{
  bool ExtendedTimeZoneActSpecificationAsync(short ScpId, TimeZoneDto dto, List<IntervalDto> intervals, int activeTime, int deactiveTime);

}
