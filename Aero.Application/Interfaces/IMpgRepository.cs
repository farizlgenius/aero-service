
using Aero.Application.DTOs;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Domain.Interfaces;

namespace Aero.Application.Interface;

public interface IMpgRepository : IBaseRepository<MonitorGroupDto,Aero.Domain.Entities.MonitorGroup>
{
      Task<int> DeleteByMacAndComponentIdAsync(string mac,short component);
      Task<int> DeleteReferenceByMacAnsComponentIdAsync(string mac,short component);
    Task<IEnumerable<MonitorGroupDto>> GetByMacAsync(string mac);
    Task<IEnumerable<Mode>> GetCommandAsync();
    Task<IEnumerable<Mode>> GetTypeAsync();
    Task<bool> IsAnyByMacAndComponentIdAsync(string mac, short component);
    Task<int> CountByMacAndUpdateTimeAsync(string mac, DateTime sync);
}
