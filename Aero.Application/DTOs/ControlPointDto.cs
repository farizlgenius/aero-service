using Aero.Domain.Entities;

namespace Aero.Application.DTOs
{

    public sealed record ControlPointDto(
        int Id,
        short CpId,
        string Name,
        int ModuleId,
        short ModuleDriverId,
        string ModuleDetail,
        short OutputNo,
        short RelayMode,
        string RelayModeDetail,
        short OfflineMode,
        string OfflineModeDetail,
        short DefaultPulse,
        int ScpId,
        int LocationId,
        bool IsActive
        ) : BaseDto(LocationId,IsActive);
}
