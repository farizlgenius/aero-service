using System;
using Aero.Application.DTOs;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Domain.Interfaces;

namespace Aero.Domain.Interface;

public interface IProcedureRepository : IBaseRepository<ProcedureDto,Procedure>
{
    Task<int> CountByDeviceIdAndUpdateTimeAsync(int device, DateTime sync);
    Task<IEnumerable<ModeDto>> GetActionTypeAsync();
    Task<short> GetLowestUnassignedNumberAsync(int max, int device);
}
