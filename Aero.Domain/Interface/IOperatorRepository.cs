using System;
using Aero.Domain.Entities;
using Aero.Domain.Interfaces;

namespace Aero.Domain.Interface;

public interface IOperatorRepository : IBaseRepository<Operator>
{
      Task<int> AddAsync(CreateOperator dto);
}
