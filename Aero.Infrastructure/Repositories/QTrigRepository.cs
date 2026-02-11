using System;
using Aero.Application.DTOs;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Aero.Infrastructure.Repositories;

public class QTrigRepository(AppDbContext context) : IQTrigRepository
{
      public async Task<int> CountByMacAndUpdateTimeAsync(string mac, DateTime sync)
      {
            var res = await context.trigger
            .AsNoTracking()
            .Where(x => x.hardware_mac.Equals(mac) && x.updated_date > sync)
            .CountAsync();

            return res;
      }

      public async Task<IEnumerable<TriggerDto>> GetAsync()
      {
            var dtos = await context.trigger
            .AsNoTracking()
            .OrderBy(x => x.component_id)
            .Select(en => new TriggerDto
            {
                  // Base
                  LocationId = en.location_id,
                  Mac = en.hardware_mac,
                  HardwareName = en.hardware.name,
                  IsActive = en.is_active,

                  // Detail
                  Name = en.name,
                  Command = en.command,
                  ProcedureId = en.procedure_id,
                  SourceNumber = en.source_number,
                  SourceType = en.source_type,
                  TranType = en.tran_type,
                  CodeMap = en.code_map.Select(x => new TransactionCodeDto
                  {
                        Name = x.name,
                        Value = x.value,
                        Description = x.description
                  }).ToList(),
                  TimeZone = en.timezone,
            }).ToArrayAsync();

            return dtos;
      }

      public async Task<TriggerDto> GetByComponentIdAsync(short componentId)
      {
            var dtos = await context.trigger
            .AsNoTracking()
            .Where(x => x.component_id == componentId)
            .OrderBy(x => x.component_id)
            .Select(en => new TriggerDto
            {
                  // Base
                  LocationId = en.location_id,
                  Mac = en.hardware_mac,
                  HardwareName = en.hardware.name,
                  IsActive = en.is_active,

                  // Detail
                  Name = en.name,
                  Command = en.command,
                  ProcedureId = en.procedure_id,
                  SourceNumber = en.source_number,
                  SourceType = en.source_type,
                  TranType = en.tran_type,
                  CodeMap = en.code_map.Select(x => new TransactionCodeDto
                  {
                        Name = x.name,
                        Value = x.value,
                        Description = x.description
                  }).ToList(),
                  TimeZone = en.timezone,
            }).FirstOrDefaultAsync();

            return dtos;
      }

      public async Task<IEnumerable<TriggerDto>> GetByLocationIdAsync(short locationId)
      {
            var dtos = await context.trigger
            .AsNoTracking()
            .Where(x => x.location_id == locationId || x.location_id == 1)
            .OrderBy(x => x.component_id)
            .Select(en => new TriggerDto
            {
                  // Base
                  LocationId = en.location_id,
                  Mac = en.hardware_mac,
                  HardwareName = en.hardware.name,
                  IsActive = en.is_active,

                  // Detail
                  Name = en.name,
                  Command = en.command,
                  ProcedureId = en.procedure_id,
                  SourceNumber = en.source_number,
                  SourceType = en.source_type,
                  TranType = en.tran_type,
                  CodeMap = en.code_map.Select(x => new TransactionCodeDto
                  {
                        Name = x.name,
                        Value = x.value,
                        Description = x.description
                  }).ToList(),
                  TimeZone = en.timezone,
            }).ToArrayAsync();

            return dtos;
      }

      public async Task<IEnumerable<Mode>> GetCodeByTranAsync(short tran)
      {
            var dtos = await context.transaction_code
                .AsNoTracking()
                .Where(x => x.transaction_type_value == tran)
                .Select(x => new Mode
                {
                    Name = x.name,
                    Description = x.description,
                    Value = x.value,
                })
                .ToArrayAsync();

                return dtos;
      }

      public async Task<IEnumerable<Mode>> GetCommandAsync()
      {
            var dtos = await context.trigger_command
                .AsNoTracking()
                .Select(x => new Mode 
                {
                    Name = x.name,
                    Description = x.description,
                    Value = x.value,
                })
                .ToArrayAsync();

            return dtos;
      }


      public async Task<short> GetLowestUnassignedNumberAsync(int max,string mac)
      {
            if (string.IsNullOrEmpty(mac))
            {
                  if (max <= 0) return -1;

                  var query = context.transaction
                      .AsNoTracking()
                      .Select(x => x.component_id);

                  // Handle empty table case quickly
                  var hasAny = await query.AnyAsync();
                  if (!hasAny)
                        return 1; // start at 1 if table is empty

                  // Load all numbers into memory (only the column, so it's lightweight)
                  var numbers = await query.Distinct().OrderBy(x => x).ToListAsync();

                  short expected = 1;
                  foreach (var num in numbers)
                  {
                        if (num != expected)
                              return expected; // found the lowest missing number
                        expected++;
                  }

                  // If none missing in sequence, return next number
                  if (expected > max) return -1;
                  return expected;
            }
            else
            {
                   if (max <= 0) return -1;

                  var query = context.trigger
                      .AsNoTracking()
                      .Where(x => x.mac == mac)
                      .Select(x => x.trig_id);

                  // Handle empty table case quickly
                  var hasAny = await query.AnyAsync();
                  if (!hasAny)
                        return 1; // start at 1 if table is empty

                  // Load all numbers into memory (only the column, so it's lightweight)
                  var numbers = await query.Distinct().OrderBy(x => x).ToListAsync();

                  short expected = 1;
                  foreach (var num in numbers)
                  {
                        if (num != expected)
                              return expected; // found the lowest missing number
                        expected++;
                  }

                  // If none missing in sequence, return next number
                  if (expected > max) return -1;
                  return expected;
            }
      }

      public async Task<IEnumerable<Mode>> GetSourceTypeAsync()
      {
            var dtos = await context.transaction_source
                .AsNoTracking()
                .Select(x => new Mode
                {
                    Name = x.name,
                    Description = x.source,
                    Value = x.value,
                })
                .ToArrayAsync();

                return dtos;
      }

      public async Task<IEnumerable<Mode>> GetTypeBySourceAsync(short source)
      {
            var dtos = await context.transaction_type
                .AsNoTracking()
                .Where(x => 
                    x.transaction_source_types.All(x => x.transction_source_value == source) &&
                    x.transaction_source_types.Any()
                )
                .Select(x => new Mode
                {
                    Name = x.name,
                    Description = "",
                    Value = x.value,
                })
                .ToArrayAsync();

            return dtos;
      }

    public async Task<Pagination<TriggerDto>> GetPaginationAsync(PaginationParamsWithFilter param,short location)
    {

        var query = context.trigger.AsNoTracking().AsQueryable();


        if (!string.IsNullOrWhiteSpace(param.Search))
        {
            if (!string.IsNullOrWhiteSpace(param.Search))
            {
                var search = param.Search.Trim();

                if (context.Database.IsNpgsql())
                {
                    var pattern = $"%{search}%";

                    query = query.Where(x =>
                        EF.Functions.ILike(x.name, pattern) ||
                        EF.Functions.ILike(x.hardware_mac, pattern) ||
                        EF.Functions.ILike(x.mac, pattern) 

                    );
                }
                else // SQL Server
                {
                    query = query.Where(x =>
                        x.name.Contains(search) ||
                        x.hardware_mac.Contains(search) ||
                        x.mac.Contains(search) 
                    );
                }
            }
        }

        query = query.Where(x => x.location_id == location || x.location_id == 1);

        if (param.StartDate != null)
        {
            var startUtc = DateTime.SpecifyKind(param.StartDate.Value, DateTimeKind.Utc);
            query = query.Where(x => x.created_date >= startUtc);
        }

        if (param.EndDate != null)
        {
            var endUtc = DateTime.SpecifyKind(param.EndDate.Value, DateTimeKind.Utc);
            query = query.Where(x => x.created_date <= endUtc);
        }

        var count = await query.CountAsync();


        var data = await query
            .AsNoTracking()
            .OrderByDescending(t => t.created_date)
            .Skip((param.PageNumber - 1) * param.PageSize)
            .Take(param.PageSize)
            .Select(en => new TriggerDto
            {
                // Base
                LocationId = en.location_id,
                Mac = en.hardware_mac,
                HardwareName = en.hardware.name,
                IsActive = en.is_active,

                // Detail
                Name = en.name,
                Command = en.command,
                ProcedureId = en.procedure_id,
                SourceNumber = en.source_number,
                SourceType = en.source_type,
                TranType = en.tran_type,
                CodeMap = en.code_map.Select(x => new TransactionCodeDto
                {
                    Name = x.name,
                    Value = x.value,
                    Description = x.description
                }).ToList(),
                TimeZone = en.timezone,
            })
            .ToListAsync();


        return new Pagination<TriggerDto>
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

    public Task<bool> IsAnyByComponentId(short component)
      {
            throw new NotImplementedException();
      }
}
