using System;
using Aero.Application.DTOs;
using Aero.Application.Interfaces;
using Aero.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Aero.Infrastructure.Repositories;

public class QTransactionRepository(AppDbContext context) : IQTransactionRepository
{
      public async Task<PaginationDto> GetPageTransactionWithCountAsync(PaginationParams param)
      {
            var query = context.transaction.AsQueryable();
            var count = await query.CountAsync();
            var data = await query
                .AsNoTracking()
                .Include(x => x.transaction_flag)
                .OrderByDescending(t => t.created_date)
                .Skip((param.PageNumber - 1) * param.PageSize)
                .Take(param.PageSize)
                .Select(en => new TransactionDto
                {
                    // Base 
                    ComponentId = en.component_id,
                    Mac = en.hardware_mac,
                    LocationId = en.location_id,

                    // extend_desc
                    Date = en.date,
                    Time = en.time,
                    SerialNumber = en.serial_number,
                    Actor = en.actor,
                    Source = en.source,
                    Origin = en.origin,
                    SourceModule = en.source_module,
                    Type = en.type,
                    TypeDesc = en.type_desc,
                    TranCode = en.tran_code,
                    Image = en.image_path,
                    TranCodeDesc = en.tran_code_desc,
                    ExtendDesc = en.extend_desc,
                    Remark = en.remark,
                    TransactionFlags = en.transaction_flag.Select(x => new TransactionFlagDto
                    {
                        Topic = x.topic,
                        Name = x.name,
                        Description = x.description
                    }).ToList(),

                })
                .ToListAsync();


            return new PaginationDto 
            {
                Data = data,
                Page = new PaginationData
                {
                    TotalCount = count,
                    PageNumber = param.PageNumber,
                    PageSize = param.PageSize,
                    TotalPage = (int)Math.Ceiling(count / (double)param.PageSize)
                }
            };
      }
}
