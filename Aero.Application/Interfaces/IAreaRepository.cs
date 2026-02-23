using System;
using Aero.Application.DTOs;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Domain.Interfaces;

namespace Aero.Application.Interface;

public interface IAreaRepository : IBaseRepository<AccessAreaDto,AccessArea,AccessArea>
{
    Task<int> CountByLocationIdAndUpdateTimeAsync(int locationId, DateTime sync);
    Task<IEnumerable<ModeDto>> GetCommandAsync();
    Task<IEnumerable<ModeDto>> GetAccessControlOptionAsync();
    Task<IEnumerable<ModeDto>> GetOccupancyControlOptionAsync();
    Task<IEnumerable<ModeDto>> GetAreaFlagOptionAsync();
    Task<IEnumerable<ModeDto>> GetMultiOccupancyOptionAsync();
}
