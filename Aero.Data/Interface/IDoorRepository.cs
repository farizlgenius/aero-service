using System;
using Aero.Domain.Entities;
using Aero.Domain.Interfaces;

namespace Aero.Domain.Interface;

public interface IDoorRepository : IBaseRepository<Door>
{
      Task<int> ChangeDoorModeAsync(string mac,short component,short acr,short mode);
}
