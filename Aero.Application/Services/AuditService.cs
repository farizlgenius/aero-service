using System;
using Aero.Application.DTOs;
using Aero.Application.Helpers;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;

namespace Aero.Application.Services;

public sealed class AuditService(IAuditRepository repo) : IAuditService
{
      public async Task CreateAuditAsync(AuditDto dto)
      {
            await repo.CreateAuditAsync(dto);
      }

      public async Task<ResponseDto<Pagination<AuditDto>>> GetPageTransactionWithCountAndDateAndSearchAsync(PaginationParamsWithFilter param,short location)
        {
            var dto = await repo.GetPageTransactionWithCountAndDateAndSearchAsync(param,location);
            return ResponseHelper.SuccessBuilder<Pagination<AuditDto>>(dto);
        }


}
