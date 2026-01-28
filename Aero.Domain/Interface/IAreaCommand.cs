using System;

namespace Aero.Domain.Interface;

public interface IAreaCommand
{
      bool ConfigureAccessArea(short ScpId, short AreaNo, short MultiOccu, short AccControl, short OccControl, short OccSet, short OccMax, short OccUp, short OccDown, short AreaFlag);
      bool GetAccessAreaStatus(short ScpId, short ComponentId, short Number);
}
