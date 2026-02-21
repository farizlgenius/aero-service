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
        Task<int> CountByMacAndUpdateTimeAsync(string mac, DateTime sync);
        Task<IEnumerable<ModuleDto>> GetByMacAsync(string mac);
        Task<bool> IsAnyByComponentAndMacAsnyc(string mac, short component);
        Task<IEnumerable<Mode>> GetBaudrateAsync();
        Task<IEnumerable<Mode>> GetProtocolAsync();
    }
}
