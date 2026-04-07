using System;

namespace Aero.Application.DTOs;

public sealed record LedConfigDto(
      short RLedId,
      short OnColor,
      short OffColor,
      short OnTime,
      short OffTime,
      short RepeatCount,
      short BeepCount
);


