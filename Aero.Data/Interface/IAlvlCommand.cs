using System;


namespace Aero.Domain.Interfaces;

public interface IAlvlCommand
{
      bool AccessLevelConfigurationExtended(short ScpId, short component, short tzAcr);
      bool AccessLevelConfigurationExtendedCreate(short ScpId, short number, List<CreateUpdateAccessLevelDoorTimeZoneDto> accessLevelDoorTimeZoneDto);
}
