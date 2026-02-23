using System;
using Aero.Application.DTOs;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Domain.Interfaces;

namespace Aero.Application.Interface;

public interface IAlvlRepository : IBaseRepository<AccessLevelDto,AccessLevel,CreateAccessLevel>
{
    Task<int> CountByLocationIdAndUpdateTimeAsync(int locationId, DateTime sync);
    Task<string> GetAcrNameByIdAndMacAsync(int id, string mac);
    Task<string> GetTimezoneNameByIdAsync(int id);
    Task<IEnumerable<AccessLevel>> GetDomainAsync();
    
}
