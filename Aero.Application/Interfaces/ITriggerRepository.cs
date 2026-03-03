using System;
using Aero.Application.DTOs;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Domain.Interfaces;

namespace Aero.Application.Interface;

public interface ITriggerRepository : IBaseRepository<TriggerDto,Trigger>
{
       Task<IEnumerable<ModeDto>> GetDeviceBySourceAsync(short location,short source);
    Task<int> CountByDeviceIdAndUpdateTimeAsync(int device, DateTime sync);
    Task<IEnumerable<ModeDto>> GetCommandAsync();
    Task<IEnumerable<ModeDto>> GetSourceTypeAsync();
    Task<IEnumerable<ModeDto>> GetCodeByTranAsync(short tran);
    Task<IEnumerable<ModeDto>> GetTypeBySourceAsync(short source);
    Task<short> GetLowestUnassignedNumberAsync(int max, int device);
}
