using System;
using Aero.Application.DTOs;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Domain.Interfaces;

namespace Aero.Application.Interface;

public interface IIntervalRepository : IBaseRepository<IntervalDto,Interval>
{
    Task<bool> IsAnyOnEachDays(CreateIntervalDto dto);
    Task<bool> IsAnyReferenceByIdAsync(int id);
    Task<IEnumerable<int>> GetTimezoneIntervalIdByIntervalIdAsync(int id);
    Task<IEnumerable<IntervalDto>> GetIntervalFromTimezoneIdAsync(int id);
    Task<int> CountByLocationIdAndUpdateTimeAsync(short locationId, DateTime sync);
}
