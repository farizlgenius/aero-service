

using Aero.Domain.Entities;

namespace Aero.Domain.Interfaces;

public interface IMpgCommand
{
      bool ConfigureMonitorPointGroup(short ScpId, short ComponentId, short nMonitor, List<MonitorGroupList> list);
      bool MonitorPointGroupArmDisarm(short ScpId, short ComponentId, short Command, short Arg1);
}
