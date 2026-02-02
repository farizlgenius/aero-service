using System;
using Aero.Domain.Entities;
using Aero.Domain.Interfaces;

namespace Aero.Domain.Interface;

public interface ITransactionRepository : IBaseRepository<Transaction>
{
      Task<Transaction> HandleTransactionAsync(IScpReply message);
}
