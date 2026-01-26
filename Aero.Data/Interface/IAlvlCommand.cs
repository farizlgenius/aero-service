using System;
using Aero.Domain.Entities;


namespace Aero.Domain.Interfaces;

public interface IAlvlCommand
{
      bool AccessLevelConfigurationExtended(short ScpId, short component, short tzAcr);
      bool AccessLevelConfigurationExtendedCreate(short ScpId, short number, List<CreateUpdateAccessLevelDoorTimeZone> accessLevelDoorTimeZoneDto);
}
