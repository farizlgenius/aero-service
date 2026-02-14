using System;

namespace Aero.Application.DTOs;

public sealed class AccessLevelComponentDto
{
    public short AlvlId { get; set; }
    public string Mac {get; set;} = string.Empty;
    public short DoorId { get; set; }
    public short AcrId { get; set; }
    public short TimezoneId { get; set; }

}
