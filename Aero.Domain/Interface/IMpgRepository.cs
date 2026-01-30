
using Aero.Domain.Interfaces;

namespace Aero.Domain.Interface;

public interface IMpgRepository : IBaseRepository<Aero.Domain.Entities.MonitorGroup>
{
      Task<int> DeleteByMacAndComponentIdAsync(string mac,short component);
      Task<int> DeleteReferenceByMacAnsComponentIdAsync(string mac,short component);
}
