using Aero.Domain.Entities;

namespace Aero.Application.DTOs;

public sealed record MonitorPointDto(int Id,int DeviceId,short DriverId, string Name, short ModuleId, string ModuleDescription, short InputNo, short InputMode, string InputModeDescription, short Debounce, short HoldTime, short LogFunction, string LogFunctionDescription, short MonitorPointMode, string MonitorPointModeDescription, short DelayEntry, short DelayExit, bool IsMask,int LocationId,bool IsActive): BaseDto(LocationId,IsActive);
