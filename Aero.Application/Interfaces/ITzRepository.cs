using System;
using Aero.Application.DTOs;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Domain.Interfaces;

namespace Aero.Application.Interface;

public interface ITzRepository : IBaseRepository<TimeZoneDto,Timezone>
{
    Task<int> CountByLocationIdAndUpdateTimeAsync(short locationId, DateTime sync);
    Task<IEnumerable<Mode>> GetCommandAsync();
    Task<IEnumerable<Mode>> GetModeAsync();
}
