using System;
using Aero.Application.DTOs;
using Aero.Application.Interfaces;
using Aero.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Aero.Infrastructure.Repositories;

public class QTransactionRepository(AppDbContext context) : IQTransactionRepository
{
    public async Task<PaginationDto> GetPageTransactionWithCountAndDateAndSearchAsync(PaginationParamsWithDate param)
    {
        var query = context.transaction.AsNoTracking().AsQueryable();
       

        if(!string.IsNullOrWhiteSpace(param.Search))
        {
            if (!string.IsNullOrWhiteSpace(param.Search))
            {
                var search = param.Search.Trim();

                if (context.Database.IsNpgsql())
                {
                    var pattern = $"%{search}%";

                    query = query.Where(x =>
                        EF.Functions.ILike(x.mac, pattern) ||
                        EF.Functions.ILike(x.actor, pattern) ||
                        EF.Functions.ILike(x.origin, pattern) ||
                        EF.Functions.ILike(x.source_desc, pattern) ||
                        EF.Functions.ILike(x.type_desc, pattern) ||
                        EF.Functions.ILike(x.tran_code_desc, pattern) ||
                        EF.Functions.ILike(x.extend_desc, pattern) ||
                        EF.Functions.ILike(x.remark, pattern) ||
                        EF.Functions.ILike(x.hardware_mac, pattern) ||
                        EF.Functions.ILike(x.hardware_name, pattern)
                    );
                }
                else // SQL Server
                {
                    query = query.Where(x =>
                        x.mac.Contains(search) ||
                        x.actor.Contains(search) ||
                        x.origin.Contains(search) ||
                        x.source_desc.Contains(search) ||
                        x.type_desc.Contains(search) ||
                        x.tran_code_desc.Contains(search) ||
                        x.extend_desc.Contains(search) ||
                        x.remark.Contains(search) ||
                        x.hardware_mac.Contains(search) ||
                        x.hardware_name.Contains(search)
                    );
                }
            }
        }

        if(param.StartDate != null)
        {
            var startUtc = DateTime.SpecifyKind(param.StartDate.Value, DateTimeKind.Utc);
            query = query.Where(x => x.date_time >= startUtc);
        }

        if (param.EndDate != null)
        {
            var endUtc = DateTime.SpecifyKind(param.EndDate.Value, DateTimeKind.Utc);
            query = query.Where(x => x.date_time <= endUtc);
        }

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
                DateTime = en.date_time.ToLocalTime(),
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
                    DateTime = en.date_time,
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
