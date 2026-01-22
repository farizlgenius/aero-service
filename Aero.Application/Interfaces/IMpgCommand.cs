using System;
using Aero.Application.DTOs;

namespace Aero.Application.Interfaces;

public interface IMpgCommand
{
      bool ConfigureMonitorPointGroup(short ScpId, short ComponentId, short nMonitor, List<MonitorGroupListDto> list);
      bool MonitorPointGroupArmDisarm(short ScpId, short ComponentId, short Command, short Arg1);
}
