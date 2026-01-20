using System;
using Aero.Domain.Entities;

namespace Aero.Domain.Interface;

public interface IHardwareRepository
{
      Task<Hardware> GetByMacAsync(string mac);
      Task<int> AddAsync(Hardware entity);
      Task<int> DeleteByMacAsync(string mac);
      Task<int> DeleteByComponentAsync(short component);
}
