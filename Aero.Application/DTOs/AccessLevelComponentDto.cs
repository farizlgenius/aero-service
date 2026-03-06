using System;

namespace Aero.Application.DTOs;


public sealed record AccessLevelComponentDto(
    short DriverId,
    short DeviceId,
    int DoorId,
    short AcrId,
    short TimeZoneId
    );
