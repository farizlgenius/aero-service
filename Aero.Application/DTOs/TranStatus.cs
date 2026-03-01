namespace Aero.Application.DTOs;

public sealed record TranStatus(string MacAddress, int Capacity, int Oldest, int LastReport, int LastLog, int Disabled, string Status);
