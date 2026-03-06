using Aero.Domain.Entities;

namespace Aero.Application.DTOs
{

    public sealed record ControlPointDto(
        int Id,
        short DriverId,
        string Name,
        int ModuleId,
        string ModuleDetail,
        short OutputNo,
        short RelayMode,
        string RelayModeDetail,
        short OfflineMode,
        string OfflineModeDetail,
        short DefaultPulse,
        short DeviceId,
        int LocationId,
        bool IsActive
        ) : BaseDto(LocationId,IsActive);
}
