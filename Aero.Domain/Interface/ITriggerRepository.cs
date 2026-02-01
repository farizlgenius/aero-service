using System;
using Aero.Domain.Entities;
using Aero.Domain.Interfaces;

namespace Aero.Domain.Interface;

public interface ITriggerRepository : IBaseRepository<Trigger>
{
       Task<IEnumerable<Mode>> GetDeviceBySourceAsync(short location,short source);
}
