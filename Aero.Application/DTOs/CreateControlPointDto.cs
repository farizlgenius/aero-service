using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aero.Application.DTOs
{
    public sealed record CreateControlPointDto(
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
        ) : BaseDto(LocationId, IsActive);
}
