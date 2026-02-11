using System;
using Aero.Application.DTOs;
using Aero.Domain.Entities;

namespace Aero.Application.Interfaces;

public interface IQTransactionRepository
{
      Task<Pagination<TransactionDto>> GetPageTransactionWithCountAsync(PaginationParams param);
    Task<Pagination<TransactionDto>> GetPageTransactionWithCountAndDateAndSearchAsync(PaginationParamsWithFilter param, short location);
}
