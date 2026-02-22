using System;
using Aero.Infrastructure.Mapper;
using Aero.Domain.Entities;
using Aero.Domain.Interface;
using Aero.Infrastructure.Data;
using Aero.Application.DTOs;
using HID.Aero.ScpdNet.Wrapper;
using Microsoft.EntityFrameworkCore;
using Aero.Application.Helpers;

namespace Aero.Infrastructure.Repositories;

public class TriggerRepository(AppDbContext context) : ITriggerRepository
{
      public async Task<int> AddAsync(Trigger data)
      {
            var en = Aero.Infrastructure.Mapper.TriggerMapper.ToEf(data);

            await context.trigger.AddAsync(en);
            return await context.SaveChangesAsync();
      }

      public async Task<int> DeleteByComponentIdAsync(short component)
      {
            throw new NotImplementedException();
      }

      public async Task<IEnumerable<Mode>> GetDeviceBySourceAsync(short location, short source)
      {
            switch (source)
            {
                  case (short)tranSrc.tranSrcScpDiag:
                  case (short)tranSrc.tranSrcScpCom:
                  case (short)tranSrc.tranSrcScpLcl:
                  case (short)tranSrc.tranSrcLoginService:
                        var dtos = await context.hardware
                            .AsNoTracking()
                            .Where(x => x.location_id == location)
                            .Select(x => new Mode
                            {
                                  Name = x.name,
                                  Value = x.component_id,
                                  Description = x.mac
                            })
                            .ToArrayAsync();
                        return dtos;
                  case (short)tranSrc.tranSrcSioDiag:
                  case (short)tranSrc.tranSrcSioCom:
                  case (short)tranSrc.tranSrcSioTmpr:
                  case (short)tranSrc.tranSrcSioPwr:
                        dtos = await context.module
                            .AsNoTracking()
                            .Where(x => x.location_id == location)
                            .Select(x => new Mode
                            {
                                  Name = x.model_desc,
                                  Value = x.component_id,
                                  Description = x.mac
                            })
                            .ToArrayAsync();
                        return dtos;
                  case (short)tranSrc.tranSrcMP:
                        dtos = await context.monitor_point
                            .AsNoTracking()
                            .Where(x => x.location_id == location)
                            .Select(x => new Mode
                            {
                                  Name = x.name,
                                  Value = x.component_id,
                                  Description = x.module.mac
                            })
                            .ToArrayAsync();
                        return dtos;
                  case (short)tranSrc.tranSrcCP:
                        dtos = await context.control_point
                            .AsNoTracking()
                            .Where(x => x.location_id == location)
                            .Select(x => new Mode
                            {
                                  Name = x.name,
                                  Value = x.component_id,
                                  Description = x.module.mac
                            })
                            .ToArrayAsync();
                        return dtos;
                  case (short)tranSrc.tranSrcACR:
                  case (short)tranSrc.tranSrcAcrTmpr:
                  case (short)tranSrc.tranSrcAcrDoor:
                  case (short)tranSrc.tranSrcAcrRex0:
                  case (short)tranSrc.tranSrcAcrRex1:
                  case (short)tranSrc.tranSrcAcrTmprAlt:
                        dtos = await context.door
                            .AsNoTracking()
                            .Where(x => x.location_id == location)
                            .Select(x => new Mode
                            {
                                  Name = x.name,
                                  Value = x.component_id,
                                  Description = x.mac
                            })
                            .ToArrayAsync();
                        return dtos;
                  case (short)tranSrc.tranSrcTimeZone:
                        dtos = await context.timezone
                            .AsNoTracking()
                            .Where(x => x.location_id == location)
                            .Select(x => new Mode
                            {
                                  Name = x.name,
                                  Value = x.component_id,
                                  Description = ""
                            })
                            .ToArrayAsync();
                        return dtos;
                  case (short)tranSrc.tranSrcProcedure:
                        dtos = await context.procedure
                            .AsNoTracking()
                            .Where(x => x.location_id == location)
                            .Select(x => new Mode
                            {
                                  Name = x.name,
                                  Value = x.component_id,
                                  Description = x.trigger.hardware_mac
                            })
                            .ToArrayAsync();
                        return dtos;
                  case (short)tranSrc.tranSrcTrigger:
                  case (short)tranSrc.tranSrcTrigVar:
                        dtos = await context.trigger
                            .AsNoTracking()
                            .Where(x => x.location_id == location)
                            .Select(x => new Mode
                            {
                                  Name = x.name,
                                  Value = x.component_id,
                                  Description = x.hardware_mac
                            })
                            .ToArrayAsync();
                        return dtos;
                  case (short)tranSrc.tranSrcMPG:
                        dtos = await context.monitor_group
                            .AsNoTracking()
                            .Where(x => x.location_id == location)
                            .Select(x => new Mode
                            {
                                  Name = x.name,
                                  Value = x.component_id,
                                  Description = x.mac
                            })
                            .ToArrayAsync();
                        return dtos;
                  case (short)tranSrc.tranSrcArea:
                        dtos = await context.area
                            .AsNoTracking()
                            .Where(x => x.location_id == location)
                            .Select(x => new Mode
                            {
                                  Name = x.name,
                                  Value = x.component_id,
                                  Description = ""
                            })
                            .ToArrayAsync();
                        return dtos;
                  default:
                        return new List<Mode>();
            }
      }

      public Task<int> UpdateAsync(Trigger newData)
      {
            throw new NotImplementedException();
      }

      Task<IEnumerable<Mode>> ITriggerRepository.GetDeviceBySourceAsync(short location, short source)
      {
            throw new NotImplementedException();
      }

    public async Task<int> CountByMacAndUpdateTimeAsync(string mac, DateTime sync)
    {
        var res = await context.trigger
        .AsNoTracking()
        .Where(x => x.mac.Equals(mac) && x.updated_date > sync)
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


    public async Task<short> GetLowestUnassignedNumberAsync(int max, string mac)
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

    public async Task<Pagination<TriggerDto>> GetPaginationAsync(PaginationParamsWithFilter param, short location)
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
                        EF.Functions.ILike(x.mac, pattern) ||
                        EF.Functions.ILike(x.mac, pattern)

                    );
                }
                else // SQL Server
                {
                    query = query.Where(x =>
                        x.name.Contains(search) ||
                        x.mac.Contains(search) ||
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
