using Aero.Domain.Entities;

namespace Aero.Application.DTOs;

public sealed record StrikeDto(int DeviceId,short DoorId,short ModuleId, short OutputNo, short RelayMode, short OfflineMode, short StrkMax, short StrkMin, short StrkMode,int LocationId,bool IsActive) : BaseDto(LocationId,IsActive);
