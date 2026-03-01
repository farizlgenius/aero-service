using Aero.Domain.Entities;

namespace Aero.Application.DTOs;

public sealed record SensorDto(int DeviceId,short ModuleId,short DoorId,short InputNo, short InputMode, short Debounce, short HoldTime, short DcHeld,int LocationId,bool IsActive) : BaseDto(LocationId,IsActive);
