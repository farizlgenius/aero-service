using System;
using Aero.Application.DTOs;
using Aero.Domain.Interfaces;

namespace Aero.Application.Interfaces;

public interface IQOperatorRepository : IBaseQueryRespository<OperatorDto>
{
      Task<bool> IsAnyByUsernameAsync(string name);
      Task<OperatorDto> GetByUsernameAsync(string username);
}
