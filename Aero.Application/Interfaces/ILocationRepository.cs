

using Aero.Application.DTOs;
using Aero.Application.Interfaces;

namespace Aero.Application.Interface;

public interface ILocationRepository : IBaseRepository<LocationDto,Aero.Domain.Entities.Location>
{
    Task<IEnumerable<LocationDto>> GetLocationsByListIdAsync(LocationRangeDto dto);
    Task<bool> IsAnyByLocationNameAsync(string name);
    Task<List<string>> CheckRelateReferenceByComponentIdAsync(short component);
}
