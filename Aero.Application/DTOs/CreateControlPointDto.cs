using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aero.Application.DTOs
{
    public sealed record CreateControlPointDto(
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
        int DeviceId,
        int LocationId,
        bool IsActive
        ) : BaseDto(LocationId, IsActive);
}
