using System;
using Aero.Domain.Entities;

namespace Aero.Application.Interfaces;

public interface ITzCommand
{
  bool ExtendedTimeZoneActSpecification(short ScpId, Domain.Entities.TimeZone dto);
  bool TimeZoneControl(short ScpId,short Component,short Command);
}
