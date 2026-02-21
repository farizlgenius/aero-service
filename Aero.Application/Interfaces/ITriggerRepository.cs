using System;
using Aero.Application.DTOs;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Domain.Interfaces;

namespace Aero.Application.Interface;

public interface ITriggerRepository : IBaseRepository<TriggerDto,Trigger>
{
       Task<IEnumerable<Mode>> GetDeviceBySourceAsync(short location,short source);
    Task<int> CountByMacAndUpdateTimeAsync(string mac, DateTime sync);
    Task<IEnumerable<Mode>> GetCommandAsync();
    Task<IEnumerable<Mode>> GetSourceTypeAsync();
    Task<IEnumerable<Mode>> GetCodeByTranAsync(short tran);
    Task<IEnumerable<Mode>> GetTypeBySourceAsync(short source);
}
