using System;
using Aero.Application.DTOs;
using Aero.Domain.Interfaces;

namespace Aero.Application.Interfaces;

public interface IQIdReportRepository : IBaseQueryRespository<IdReportDto>
{
      Task<int> GetCountAsync();
}
