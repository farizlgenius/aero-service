using System;
using Aero.Application.DTOs;
using Aero.Domain.Entities;
using Aero.Domain.Interfaces;

namespace Aero.Domain.Interface;

public interface IQMpgRepository : IBaseQueryRespository<MonitorGroupDto>
{
      Task<IEnumerable<Mode>> GetCommandAsync();
      Task<IEnumerable<Mode>> GetTypeAsync();
      Task<bool> IsAnyByMacAndComponentIdAsync(string mac,short component);

}
