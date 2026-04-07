namespace Aero.Application.DTOs;

public sealed record TranStatus(int ScpId, int Capacity, int Oldest, int LastReport, int LastLog, int Disabled, string Status);
