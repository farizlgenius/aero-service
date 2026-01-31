using System;
using Aero.Domain.Entities;
using Aero.Domain.Interfaces;

namespace Aero.Domain.Interface;

public interface IOperatorRepository : IBaseRepository<Operator>
{
      Task<int> AddAsync(CreateOperator dto);
      Task<int> UpdateAsync(CreateOperator dto);
      Task<int> UpdatePasswordAsync(string username,string password);
}
