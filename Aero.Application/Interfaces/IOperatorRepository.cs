using System;
using Aero.Application.DTOs;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Domain.Interfaces;

namespace Aero.Domain.Interface;

public interface IOperatorRepository : IBaseRepository<OperatorDto,Operator>
{
      Task<int> AddAsync(CreateOperator dto);
      Task<int> UpdateAsync(CreateOperator dto);
      Task<int> UpdatePasswordAsync(string username,string password);
    Task<bool> IsAnyByUsernameAsync(string name);
    Task<OperatorDto> GetByUsernameAsync(string username);
    Task<string> GetPasswordByUsername(string username);
}
