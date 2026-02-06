using System;
using Aero.Application.DTOs;

namespace Aero.Application.Interfaces;

public interface IQTransactionRepository
{
      Task<PaginationDto> GetPageTransactionWithCountAsync(PaginationParams param);
    Task<PaginationDto> GetPageTransactionWithCountAndDateAndSearchAsync(PaginationParamsWithDate param);
}
