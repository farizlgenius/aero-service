using System;

namespace Aero.Application.DTOs;


public sealed record AccessLevelComponentDto(
    short DriverId,
    string Mac,
    short DoorId,
    short AcrId,
    short TimeZoneId
    );
