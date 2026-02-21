using System;
using Aero.Application.DTOs;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Domain.Interfaces;

namespace Aero.Application.Interface;

public interface IAreaRepository : IBaseRepository<AccessAreaDto,AccessArea>
{
    Task<int> CountByLocationIdAndUpdateTimeAsync(short locationId, DateTime sync);
    Task<IEnumerable<Mode>> GetCommandAsync();
    Task<IEnumerable<Mode>> GetAccessControlOptionAsync();
    Task<IEnumerable<Mode>> GetOccupancyControlOptionAsync();
    Task<IEnumerable<Mode>> GetAreaFlagOptionAsync();
    Task<IEnumerable<Mode>> GetMultiOccupancyOptionAsync();
}
