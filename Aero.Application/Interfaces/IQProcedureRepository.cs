using System;
using Aero.Application.DTOs;
using Aero.Domain.Entities;
using Aero.Domain.Interfaces;

namespace Aero.Application.Interfaces;

public interface IQProcedureRepository : IBaseQueryRespository<ProcedureDto>
{
      Task<IEnumerable<Mode>> GetActionTypeAsync();
}
