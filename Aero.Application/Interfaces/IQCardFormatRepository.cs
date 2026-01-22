using System;
using Aero.Application.DTOs;
using Aero.Domain.Interfaces;

namespace Aero.Application.Interfaces;

public interface IQCfmtRepository : IBaseQueryRespository<CardFormatDto>
{
      Task<int> CountByUpdateTimeAsync(DateTime sync);
}
