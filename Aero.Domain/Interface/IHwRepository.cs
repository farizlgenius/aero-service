using System;
using Aero.Domain.Entities;
using Aero.Domain.Interfaces;

namespace Aero.Domain.Interface;

public interface IHwRepository : IBaseRepository<Hardware>
{
      Task<Hardware> GetByMacAsync(string mac);
      Task<int> DeleteByMacAsync(string mac);
      Task<int> DeleteByComponentAsync(short component);
      Task<int> UpdateSyncStatusByMacAsync(string mac);
      Task<int> UpdateVerifyMemoryAllocateByComponentIdAsync(short component,bool isSync);
      Task<int> UpdateVerifyHardwareCofigurationMyMacAsync(string mac,bool status);
      Task UpdateIpAddressAsync(int ScpId,string ip);
      Task UpdatePortAddressAsync(int ScpId,string port);
      
      
}
