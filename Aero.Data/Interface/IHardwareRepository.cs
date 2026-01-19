using System;
using Aero.Domain.Entities;

namespace Aero.Domain.Interface;

public interface IHardwareRepository
{
    Task<IEnumerable<Hardware>> GetAsync();
}
