using System;
using Aero.Application.DTOs;
using Aero.Domain.Entities;
using Aero.Domain.Interfaces;

namespace Aero.Application.Interfaces;

public interface IQHwRepository : IBaseQueryRespository<HardwareDto>
{
    Task<short> GetLocationIdFromMacAsync(string mac);
      Task<IEnumerable<short>> GetComponentIdsByLocationIdAsync(short locationid);
    Task<IEnumerable<string>> GetMacsByLocationIdAsync(short locationid);
    Task<HardwareDto> GetByMacAsync(string mac);
      Task<bool> IsAnyByMac(string mac);
      Task<bool> IsAnyByMacAndComponent(string mac,short component);
      Task<short> GetComponentIdFromMacAsync(string mac);
      Task<string> GetMacFromComponentAsync(short component);
      Task<bool> IsAnyModuleReferenceByMacAsync(string mac);
      Task<ScpSetting> GetScpSettingAsync();
      Task<IEnumerable<(short ComponentId, string Mac)>> GetComponentAndMacAsync(); 
      Task<IEnumerable<Mode>> GetHardwareTypeAsync();
      Task<IEnumerable<short>> GetComponentIdByLocationIdAsync(short location);
      Task<IEnumerable<string>> GetMacsAsync();
      Task<IEnumerable<short>> GetComponentIdsAsync();
      Task<List<MemoryDto>> CheckAllocateMemoryAsync(IScpReply message);
      Task AssignPortAsync(IScpReply message);
      Task AssignIpAddressAsync(IScpReply message);
      Task<string> GetNameByComponentIdAsync(short component);
      

}
