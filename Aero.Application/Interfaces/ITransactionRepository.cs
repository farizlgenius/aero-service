using Aero.Application.DTOs;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Domain.Interfaces;
using System;

namespace Aero.Application.Interface;

public interface ITransactionRepository : IBaseRepository<TransactionDto,Transaction>
{
      Task<Transaction> HandleTransactionAsync(IScpReply message);
    Task<Pagination<TransactionDto>> GetPageTransactionWithCountAsync(PaginationParams param);
    Task<Pagination<TransactionDto>> GetPageTransactionWithCountAndDateAndSearchAsync(PaginationParamsWithFilter param, short location);
    IEnumerable<Mode> GetSourceAsync();
    Task<IEnumerable<Mode>> GetDeviceAsync(int source);
}
