using Aero.Domain.Entities;

namespace Aero.Application.DTOs
{

    public sealed record ControlPointDto(
        short DriverId,
        string Name,
        short ModuleId,
        string ModuleDetail,
        short OutputNo,
        short RelayMode,
        string RelayModeDetail,
        short OfflineMode,
        string OfflineModeDetail,
        short DefaultPulse,
        int LocationId,
        bool IsActive
        ) : BaseDto(LocationId,IsActive);
}
