using System;
using Aero.Application.DTOs;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Domain.Interfaces;

namespace Aero.Application.Interface;

public interface IAlvlRepository : IBaseRepository<AccessLevelDto,AccessLevel>
{
    Task<int> CountByLocationIdAndUpdateTimeAsync(int locationId, DateTime sync);
    Task<string> GetAcrNameByIdAndDeviceIdAsync(int id, int deviceId);
    Task<string> GetTimezoneNameByIdAsync(int id);
    Task<IEnumerable<AccessLevel>> GetDomainAsync();
    Task<short> GetLowestUnassignedNumberAsync(int max, int driverid);



}
