using System;
using Aero.Application.DTOs;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Domain.Interfaces;

namespace Aero.Application.Interface;

public interface IHolRepository : IBaseRepository<HolidayDto,Holiday>
{
      Task<int> RemoveAllAsync();
      //Task<Aero.Domain.Entities.Holiday> GetByIdAsync(int id);
    Task<int> CountByLocationIdAndUpdateTimeAsync(int locationId, DateTime sync);
    Task<bool> IsAnyWithSameDataAsync(short day, short month, short year);
    Task<short> GetLowestUnassignedNumberAsync(int max);
}
