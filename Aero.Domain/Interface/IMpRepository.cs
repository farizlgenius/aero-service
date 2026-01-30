using System;
using Aero.Domain.Entities;
using Aero.Domain.Interfaces;

namespace Aero.Domain.Interface;

public interface IMpRepository : IBaseRepository<MonitorPoint>
{
      Task<int> SetMaskAsync(string mac,short mpid,bool mask);
}
