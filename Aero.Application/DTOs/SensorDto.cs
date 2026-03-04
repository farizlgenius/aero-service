using Aero.Domain.Entities;

namespace Aero.Application.DTOs;

public sealed record SensorDto(short DeviceId,int ModuleId,short ModuleDriverId,int DoorId,short InputNo, short InputMode, short Debounce, short HoldTime, short DcHeld,int LocationId,bool IsActive) : BaseDto(LocationId,IsActive);
