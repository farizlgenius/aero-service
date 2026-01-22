using System;

namespace Aero.Application.DTOs;

public class MemoryAllocateDto
{
      public string Mac { get; set;} = string.Empty;
      public List<MemoryDto> Memories {get; set;} = new List<MemoryDto>();

}
