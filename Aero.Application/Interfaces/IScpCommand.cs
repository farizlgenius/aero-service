using System;
using Aero.Domain.Entities;

namespace Aero.Application.Interfaces;

public interface IScpCommand
{
      bool DetachScp(short component);
      bool ResetScp(short component);
      bool ScpDeviceSpecification(short component, ScpSetting setting);
      bool AccessDatabaseSpecification(short ScpId, ScpSetting setting);
      bool TimeSet(short component);
      bool ReadStructureStatus(short component);
}
