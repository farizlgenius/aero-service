namespace Aero.Application.DTOs;

public sealed record MonitorGroupCommandDto(string Mac, short ComponentId, short Command, short Arg);
