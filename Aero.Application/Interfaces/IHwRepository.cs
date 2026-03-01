using System;
using Aero.Application.DTOs;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Domain.Interfaces;

namespace Aero.Application.Interface;

public interface IHwRepository : IBaseRepository<HardwareDto, Device>
{
  Task<Device> GetDomainByMacAsync(string mac);
  Task<int> DeleteByMacAsync(string mac);
  Task<int> DeleteByIdAsync(short component);
  Task<int> UpdateSyncStatusByMacAsync(string mac);
  Task<int> UpdateVerifyMemoryAllocateByComponentIdAsync(short component, bool isSync);
  Task<int> UpdateVerifyHardwareCofigurationMyMacAsync(string mac, bool status);
  Task UpdateIpAddressAsync(int ScpId, string ip);
  Task UpdatePortAddressAsync(int ScpId, string port);
  Task<short> GetLocationIdFromMacAsync(string mac);
  Task<IEnumerable<short>> GetDriverIdsByLocationIdAsync(short locationid);
  Task<IEnumerable<string>> GetMacsByLocationIdAsync(short locationid);
  Task<HardwareDto> GetByMacAsync(string mac);
  Task<bool> IsAnyByMac(string mac);
  Task<bool> IsAnyByMacAndComponent(string mac, short component);
  Task<short> GetComponentIdFromMacAsync(string mac);
  Task<string> GetMacFromComponentAsync(short component);
  Task<bool> IsAnyModuleReferenceByMacAsync(string mac);
  Task<ScpSetting> GetScpSettingAsync();
  Task<IEnumerable<(short ComponentId, string Mac)>> GetComponentAndMacAsync();
  Task<IEnumerable<Mode>> GetHardwareTypeAsync();
  Task<IEnumerable<short>> GetDriverIdByLocationIdAsync(int location);
  Task<IEnumerable<string>> GetMacsAsync();
  Task<IEnumerable<short>> GetComponentIdsAsync();
  Task<List<MemoryDto>> CheckAllocateMemoryAsync(IScpReply message);
  Task AssignPortAsync(IScpReply message);
  Task AssignIpAddressAsync(IScpReply message);
  Task<string> GetNameByComponentIdAsync(short component);


}
