using Aero.Domain.Entities;
using Aero.Domain.Enums;

namespace Aero.Application.DTOs;

public sealed record ReaderDto(int DeviceId,short ModuleId,short DoorId, short ReaderNo, short DataFormat, short KeypadMode, short LedDriveMode, bool OsdpFlag, short OsdpBaudrate, short OsdpDiscover, short OsdpTracing, short OsdpAddress, short OsdpSecureChannel,int LocationId,bool IsActive) : BaseDto(LocationId,IsActive);
