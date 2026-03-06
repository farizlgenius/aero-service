using Aero.Application.DTOs;
using Aero.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aero.Application.Interfaces
{
    public interface IModuleRepository : IBaseRepository<ModuleDto,Module>
    {
        Task<int> CountByDeviceIdAndUpdateTimeAsync(int device, DateTime sync);
        Task<IEnumerable<ModuleDto>> GetByDeviceIdAsync(int device);
        Task<bool> IsAnyByDriverAndDeviceIdAsnyc(int device, short driver);
        Task<IEnumerable<ModeDto>> GetBaudrateAsync();
        Task<IEnumerable<ModeDto>> GetProtocolAsync();
        Task<IEnumerable<ModuleDto>> GetAnyByDeviceId(int device);
        Task<short> GetLowestUnassignedNumberAsync(int max, int device);
    }
}
