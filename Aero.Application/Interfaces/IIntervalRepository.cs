using System;
using Aero.Application.DTOs;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Domain.Interfaces;

namespace Aero.Application.Interface;

public interface IIntervalRepository : IBaseRepository<IntervalDto,Interval>
{
    Task<bool> IsAnyOnEachDays(IntervalDto dto);
    Task<bool> IsAnyReferenceByComponentAsync(short component);
    Task<IEnumerable<short>> GetTimezoneIntervalIdByIntervalComponentIdAsync(short component);
    Task<IEnumerable<IntervalDto>> GetIntervalFromTimezoneComponentIdAsync(short component);
    Task<int> CountByLocationIdAndUpdateTimeAsync(short locationId, DateTime sync);
}
