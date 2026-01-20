using System;
using Aero.Application.DTOs;

namespace Aero.Application.Interfaces;

public interface IBaseQueryRespository<T>
{
      Task<HardwareDto> GetByMacAsync(string mac);
      Task<bool> IsAnyByMac(string mac);
      Task<bool> IsAnyByComponet(short component);
      Task<IEnumerable<T>> GetAsync();
      Task<T> GetByComponentIdAsync(short componentId);
      Task<IEnumerable<T>> GetByLocationIdAsync(short locationId);

}
