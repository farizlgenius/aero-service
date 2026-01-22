using System;
using Aero.Application.DTOs;

namespace Aero.Application.Interfaces;

public interface IAlvlCommand
{
      bool AccessLevelConfigurationExtended(short ScpId, short component, short tzAcr);
      bool AccessLevelConfigurationExtendedCreate(short ScpId, short number, List<CreateUpdateAccessLevelDoorTimeZoneDto> accessLevelDoorTimeZoneDto);
}
