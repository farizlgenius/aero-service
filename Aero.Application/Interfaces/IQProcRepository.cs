using System;
using Aero.Application.DTOs;
using Aero.Domain.Interfaces;

namespace Aero.Application.Interfaces;

public interface IQProcRepository : IBaseQueryRespository<ProcedureDto>
{
      Task<int> CountByMacAndUpdateTimeAsync(string mac,DateTime sync);
}
