using System;

namespace Aero.Application.DTOs;

public sealed record MemoryDto(short nStrType, string StrType, int nRecord, int nRecSize, int nActive, int nSwAlloc, int nSwRecord, bool IsSync);
