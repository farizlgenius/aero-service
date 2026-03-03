using System;
using Aero.Application.DTOs;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Domain.Interfaces;

namespace Aero.Application.Interface;

public interface IDeviceRepository : IBaseRepository<DeviceDto, Device>
{
  Task<DeviceComponent> GetDeviceComponentByModelAsync(short model);
  Task<Device> GetDomainByMacAsync(string mac);
  Task<int> DeleteByMacAsync(string mac);
  Task<int> UpdateSyncStatusByIdAsync(int device);
  Task<int> UpdateVerifyMemoryAllocateByIdAsync(int device, bool isSync);
  Task<int> UpdateVerifyHardwareCofigurationMyMacAsync(string mac, bool status);
  Task UpdateIpAddressAsync(int ScpId, string ip);
  Task UpdatePortAddressAsync(int ScpId, string port);
  Task<int> GetLocationIdFromDriverIdAsync(int device);
  Task<IEnumerable<short>> GetDriverIdsByLocationIdAsync(int locationid);
  Task<IEnumerable<string>> GetMacsByLocationIdAsync(int locationid);
  Task<DeviceDto> GetByMacAsync(string mac);
  Task<bool> IsAnyByMac(string mac);
  Task<bool> IsAnyByMacAndDriver(string mac, int driver);
  Task<short> GetComponentIdFromMacAsync(string mac);
  Task<string> GetMacFromComponentAsync(short component);
  Task<bool> IsAnyModuleReferenceByDriverIdAsync(int driver);
  Task<ScpSetting> GetScpSettingAsync();
  Task<IEnumerable<(short DriverId, string Mac)>> GetDriverAndMacAsync();
  Task<IEnumerable<short>> GetDriverIdByLocationIdAsync(int location);
  Task<IEnumerable<string>> GetMacsAsync();
  Task<IEnumerable<short>> GetDriverIdsAsync();
  Task<List<MemoryDto>> CheckAllocateMemoryAsync(IScpReply message);
  Task AssignPortAsync(IScpReply message);
  Task AssignIpAddressAsync(IScpReply message);
  Task<string> GetNameByDriverIdAsync(int device);
Task<short> GetLowestUnassignedNumberAsync(int max);

}
