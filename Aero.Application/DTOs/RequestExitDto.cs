using Aero.Domain.Entities;

namespace Aero.Application.DTOs;

public sealed record RequestExitDto(int DeviceId,short ModuleId,short DoorId, short InputNo, short InputMode, short Debounce, short HoldTime, short MaskTimeZone,int LocationId,bool IsActive) : BaseDto(LocationId,IsActive);
