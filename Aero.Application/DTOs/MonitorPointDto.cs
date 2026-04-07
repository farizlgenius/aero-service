using Aero.Domain.Entities;

namespace Aero.Application.DTOs;

public sealed record MonitorPointDto(
      int Id,
      int ScpId,
      short MpId, 
      string Name, 
      int ModuleId,
      short ModuleDriverId, 
      string ModuleDescription, 
      short InputNo, 
      short InputMode, 
      string InputModeDescription, 
      short Debounce, 
      short HoldTime, 
      short LogFunction, 
      string LogFunctionDescription, 
      short MonitorPointMode, 
      string MonitorPointModeDescription, 
      short DelayEntry, 
      short DelayExit, 
      bool IsMask,int LocationId,bool IsActive): BaseDto(LocationId,IsActive);
