using System;
using Aero.Application.DTOs;
using Aero.Domain.Entities;
using Aero.Domain.Interfaces;

namespace Aero.Application.Interfaces;

public interface IQTrigRepository : IBaseQueryRespository<TriggerDto>
{
      Task<int> CountByMacAndUpdateTimeAsync(string mac,DateTime sync);
      Task<IEnumerable<Mode>> GetCommandAsync();
      Task<IEnumerable<Mode>> GetSourceTypeAsync();
      Task<IEnumerable<Mode>> GetCodeByTranAsync(short tran);
      Task<IEnumerable<Mode>> GetTypeBySourceAsync(short source);
     

}
