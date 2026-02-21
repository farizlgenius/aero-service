using System;
using Aero.Application.DTOs;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Domain.Interfaces;

namespace Aero.Application.Interface;

public interface IAlvlRepository : IBaseRepository<AccessLevelDto,AccessLevel>
{
      Task<int> AddCreateAsync(AccessLevel domain);
      Task<int> UpdateCreateAsync(AccessLevel domain);
    Task<int> CountByLocationIdAndUpdateTimeAsync(short locationId, DateTime sync);
    Task<string> GetACRNameByComponentIdAndMacAsync(short component, string mac);
    Task<string> GetTimezoneNameByComponentIdAsync(short component);
    Task<IEnumerable<AccessLevel>> GetDomainAsync();
}
