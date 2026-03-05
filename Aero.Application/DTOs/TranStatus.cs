namespace Aero.Application.DTOs;

public sealed record TranStatus(int DriverId, int Capacity, int Oldest, int LastReport, int LastLog, int Disabled, string Status);
