using System;
using Aero.Application.DTOs;
using Aero.Domain.Entities;

namespace Aero.Application.Interfaces;

public interface IAuditService
{
      Task<ResponseDto<Pagination<AuditDto>>> GetPageTransactionWithCountAndDateAndSearchAsync(PaginationParamsWithFilter param,short location);
      Task CreateAuditAsync(AuditDto dto);
}
