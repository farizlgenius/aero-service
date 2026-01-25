using System;
using Aero.Domain.Entities;

namespace Aero.Domain.Interfaces;

public interface IHolCommand
{
      bool HolidayConfiguration(Holiday domain, short ScpId);
      bool DeleteHolidayConfiguration(Holiday domain, short ScpId);
     bool ClearHolidayConfiguration(short ScpId);
}
