using System;
using Aero.Domain.Entities;
using Aero.Domain.Interfaces;

namespace Aero.Application.Interfaces;

public interface IIdReportRepository : IBaseRepository<IdReport>
{
      Task<IdReport> GetByMacAndComponentIdAsync(string mac,short component);
      Task<int> DeleteByMacAndComponentIdAsync(string mac,short component);
}
