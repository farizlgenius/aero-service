using System;
using Aero.Application.DTOs;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Domain.Interfaces;

namespace Aero.Domain.Interface;

public interface IProcedureRepository : IBaseRepository<ProcedureDto,Procedure>
{
    Task<int> CountByMacAndUpdateTimeAsync(string mac, DateTime sync);
    Task<IEnumerable<Mode>> GetActionTypeAsync();
}
