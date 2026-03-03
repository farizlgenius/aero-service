
using Aero.Application.DTOs;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Domain.Interfaces;

namespace Aero.Application.Interface;

public interface IMpgRepository : IBaseRepository<MonitorGroupDto,Aero.Domain.Entities.MonitorGroup>
{
    // Task<IEnumerable<MonitorGroupDto>> GetByMacAsync(string mac);
    Task<IEnumerable<ModeDto>> GetCommandAsync();
    Task<IEnumerable<ModeDto>> GetTypeAsync();
    Task<int> CountByDriverIdAndUpdateTimeAsync(int driverid, DateTime sync);
    Task<short> GetLowestUnassignedNumberAsync(int max, int device);
    Task<IEnumerable<MonitorGroupDto>> GetByDeviceIdAsync(int device);

    Task<int> DeleteReferenceByIdAsync(int Id);
}
