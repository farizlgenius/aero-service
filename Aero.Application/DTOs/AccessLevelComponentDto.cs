using System;

namespace Aero.Application.DTOs;


public sealed record AccessLevelComponentDto(
    short AlvlId,
    int DeviceId,
    int DoorId,
    short AcrId,
    short TimeZoneId
    );
