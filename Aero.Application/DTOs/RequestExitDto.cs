using Aero.Domain.Entities;

namespace Aero.Application.DTOs;

public sealed record RequestExitDto(short DeviceId,int ModuleId,short ModuleDriverId,int DoorId, short InputNo, short InputMode, short Debounce, short HoldTime, short MaskTimeZone,int LocationId,bool IsActive) : BaseDto(LocationId,IsActive);
