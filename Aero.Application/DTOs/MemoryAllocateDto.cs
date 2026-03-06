using System;

namespace Aero.Application.DTOs;

public record MemoryAllocateDto(string Mac, List<MemoryDto> Memories);
