using System;

namespace Aero.Application.DTOs;

public sealed class AccessLevelComponentDto
{
      public string Mac {get; set;} = string.Empty;
      public List<AccessLevelDoorComponentDto> DoorComponent {get; set;} = new List<AccessLevelDoorComponentDto>();

}
