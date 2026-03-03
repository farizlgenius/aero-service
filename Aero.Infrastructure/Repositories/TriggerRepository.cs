using System;
using Aero.Infrastructure.Mapper;
using Aero.Domain.Entities;
using Aero.Domain.Interface;
using Aero.Infrastructure.Persistences;
using Aero.Application.DTOs;
using HID.Aero.ScpdNet.Wrapper;
using Microsoft.EntityFrameworkCore;
using Aero.Application.Helpers;
using Aero.Application.Interface;
using System.Security.Cryptography.X509Certificates;

namespace Aero.Infrastructure.Repositories;

public class TriggerRepository(AppDbContext context) : ITriggerRepository
{
      public async Task<int> AddAsync(Trigger data)
      {
            var en = new Persistences.Entities.Trigger(data);

            await context.trigger.AddAsync(en);
            var rec = await context.SaveChangesAsync();
            if(rec <= 0) return -1;
            return en.id;
      }

      public async Task<int> DeleteByIdAsync(int id)
      {
            throw new NotImplementedException();
      }

      public async Task<IEnumerable<ModeDto>> GetDeviceBySourceAsync(short location, short source)
      {
            switch (source)
            {
                  case (short)tranSrc.tranSrcScpDiag:
                  case (short)tranSrc.tranSrcScpCom:
                  case (short)tranSrc.tranSrcScpLcl:
                  case (short)tranSrc.tranSrcLoginService:
                        var dtos = await context.device
                            .AsNoTracking()
                            .Where(x => x.location_id == location)
                            .Select(x => new ModeDto(x.name,x.driver_id,x.mac))
                            .ToArrayAsync();
                        return dtos;
                  case (short)tranSrc.tranSrcSioDiag:
                  case (short)tranSrc.tranSrcSioCom:
                  case (short)tranSrc.tranSrcSioTmpr:
                  case (short)tranSrc.tranSrcSioPwr:
                        dtos = await context.module
                            .AsNoTracking()
                            .Where(x => x.location_id == location)
                            .Select(x => new ModeDto(x.model_detail,x.driver_id,""))
                            .ToArrayAsync();
                        return dtos;
                  case (short)tranSrc.tranSrcMP:
                        dtos = await context.monitor_point
                            .AsNoTracking()
                            .Where(x => x.location_id == location)
                            .Select(x => new ModeDto(x.name,x.driver_id,""))
                            .ToArrayAsync();
                        return dtos;
                  case (short)tranSrc.tranSrcCP:
                        dtos = await context.control_point
                            .AsNoTracking()
                            .Where(x => x.location_id == location)
                            .Select(x => new ModeDto(x.name,x.driver_id,""))
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
                            .Select(x => new ModeDto(x.name,x.driver_id,""))
                            .ToArrayAsync();
                        return dtos;
                  case (short)tranSrc.tranSrcTimeZone:
                        dtos = await context.timezone
                            .AsNoTracking()
                            .Where(x => x.location_id == location)
                            .Select(x => new ModeDto(x.name,x.driver_id,""))
                            .ToArrayAsync();
                        return dtos;
                  case (short)tranSrc.tranSrcProcedure:
                        dtos = await context.procedure
                            .AsNoTracking()
                            .Where(x => x.location_id == location)
                            .Select(x => new ModeDto(x.name,x.driver_id,""))
                            .ToArrayAsync();
                        return dtos;
                  case (short)tranSrc.tranSrcTrigger:
                  case (short)tranSrc.tranSrcTrigVar:
                        dtos = await context.trigger
                            .AsNoTracking()
                            .Where(x => x.location_id == location)
                            .Select(x => new ModeDto(x.name,x.driver_id,""))
                            .ToArrayAsync();
                        return dtos;
                  case (short)tranSrc.tranSrcMPG:
                        dtos = await context.monitor_group
                            .AsNoTracking()
                            .Where(x => x.location_id == location)
                            .Select(x => new ModeDto(x.name,x.driver_id,""))
                            .ToArrayAsync();
                        return dtos;
                  case (short)tranSrc.tranSrcArea:
                        dtos = await context.area
                            .AsNoTracking()
                            .Where(x => x.location_id == location)
                            .Select(x => new ModeDto(x.name,x.driver_id,""))
                            .ToArrayAsync();
                        return dtos;
                  default:
                        return new List<ModeDto>();
            }
      }

      public Task<int> UpdateAsync(Trigger newData)
      {
            throw new NotImplementedException();
      }


    public async Task<int> CountByDeviceIdAndUpdateTimeAsync(int device, DateTime sync)
    {
        var res = await context.trigger
        .AsNoTracking()
        .Where(x => x.device_id == device && x.updated_date > sync)
        .CountAsync();

        return res;
    }

    public async Task<IEnumerable<TriggerDto>> GetAsync()
    {
        var dtos = await context.trigger
        .AsNoTracking()
        .OrderBy(x => x.id)
        .Select(en => new TriggerDto(en.id,en.device_id,en.driver_id,en.name,en.command,en.procedure_id,en.source_type,en.source_number,en.tran_type,en.code_map.Select(x => new TransactionCodeDto(x.name,x.value,x.description)).ToList()
            ,en.timezone,en.location_id,en.is_active)).ToArrayAsync();

        return dtos;
    }

    public async Task<TriggerDto> GetByIdAsync(int id)
    {
        var dtos = await context.trigger
        .AsNoTracking()
        .Where(x => x.id == id)
        .OrderBy(x => x.id)
        .Select(en => new TriggerDto(en.id,en.device_id,en.driver_id,en.name,en.command,en.procedure_id,en.source_type,en.source_number,en.tran_type,en.code_map.Select(x => new TransactionCodeDto(x.name,x.value,x.description)).ToList()
            ,en.timezone,en.location_id,en.is_active))
            .FirstOrDefaultAsync();

        return dtos;
    }

    public async Task<IEnumerable<TriggerDto>> GetByLocationIdAsync(int locationId)
    {
        var dtos = await context.trigger
        .AsNoTracking()
        .Where(x => x.location_id == locationId || x.location_id == 1)
        .OrderBy(x => x.id)
        .Select(en => new TriggerDto(en.id,en.device_id,en.driver_id,en.name,en.command,en.procedure_id,en.source_type,en.source_number,en.tran_type,
        en.code_map.Select(x => new TransactionCodeDto(x.name,x.value,x.description))
            .ToList()
        ,en.timezone,
        en.location_id,en.is_active
        ))
        .ToArrayAsync();

        return dtos;
    }

    public async Task<IEnumerable<ModeDto>> GetCodeByTranAsync(short tran)
    {
        var dtos = await context.transaction_code
            .AsNoTracking()
            .Where(x => x.transaction_type_value == tran)
            .Select(x => new ModeDto(x.name,x.value,x.description))
            .ToArrayAsync();

        return dtos;
    }

    public async Task<IEnumerable<ModeDto>> GetCommandAsync()
    {
        var dtos = await context.trigger_command
            .AsNoTracking()
            .Select(x => new ModeDto(x.name,x.value,x.description))
            .ToArrayAsync();

        return dtos;
    }


    public async Task<short> GetLowestUnassignedNumberAsync(int max, int device)
    {
        if (max <= 0) return -1;

            var query = context.trigger
                .AsNoTracking()
                .Where(x => x.device_id == device)
                .Select(x => x.driver_id);

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

    public async Task<IEnumerable<ModeDto>> GetSourceTypeAsync()
    {
        var dtos = await context.transaction_source
            .AsNoTracking()
            .Select(x => new ModeDto(x.name,x.value,""))
            .ToArrayAsync();

        return dtos;
    }

    public async Task<IEnumerable<ModeDto>> GetTypeBySourceAsync(short source)
    {
        var dtos = await context.transaction_type
            .AsNoTracking()
            .Where(x =>
                x.transaction_source_types.All(x => x.transction_source_value == source) &&
                x.transaction_source_types.Any()
            )
            .Select(x => new ModeDto(x.name,x.value,""))
            .ToArrayAsync();

        return dtos;
    }

    public async Task<Pagination<TriggerDto>> GetPaginationAsync(PaginationParamsWithFilter param, int location)
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
                        EF.Functions.ILike(x.name, pattern) 

                    );
                }
                else // SQL Server
                {
                    query = query.Where(x =>
                        x.name.Contains(search) 
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
            .Select(en => new TriggerDto(en.id,en.device_id,en.driver_id,en.name,en.command,en.procedure_id,en.source_type,en.source_number,en.tran_type,en.code_map.Select(x => new TransactionCodeDto(x.name,x.value,x.description)).ToList()
            ,en.timezone,en.location_id,en.is_active))
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

    public Task<bool> IsAnyByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsAnyByNameAsync(string name)
    {
        throw new NotImplementedException();
    }
}
