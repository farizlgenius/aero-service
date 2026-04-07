using System;

namespace Aero.Application.DTOs;

public sealed record LedDto(
      int Id,
      short LedMode,
      List<LedConfigDto> Config,
      int LocationId,
      bool IsActive
      ) : BaseDto(LocationId,IsActive);
