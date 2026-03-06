using Aero.Application.DTOs;
using Aero.Domain.Entities;
using Aero.Domain.Interface;
using Aero.Infrastructure.Persistences;
using Aero.Infrastructure.Mapper;
using Microsoft.EntityFrameworkCore;
using System;

namespace Aero.Infrastructure.Repositories;

public sealed class ProcRepository(AppDbContext context) : IProcedureRepository
{
    public async Task<int> AddAsync(Procedure data)
    {
        var en = new Aero.Infrastructure.Persistences.Entities.Procedure(data);
        await context.procedure.AddAsync(en);
        var rec = await context.SaveChangesAsync();
        if(rec <= 0) return -1;
        return en.id;
    }

    public async Task<int> DeleteByIdAsync(int id)
    {
        var en = await context.procedure
        .Where(x => x.id == id)
        .OrderBy(x => x.id)
        .FirstOrDefaultAsync();

        if (en is null) return 0;

        context.procedure.Remove(en);
        return await context.SaveChangesAsync();
    }

    public async Task<int> UpdateAsync(Procedure data)
    {
        var en = await context.procedure
        .Include(x => x.actions)
        .Where(x => x.id == data.Id)
        .OrderBy(x => x.id)
        .FirstOrDefaultAsync();

        if (en is null) return 0;

        context.action.RemoveRange(en.actions);

        en.Update(data);

        context.procedure.Update(en);
        return await context.SaveChangesAsync();


    }

    public async Task<int> CountByDeviceIdAndUpdateTimeAsync(int device, DateTime sync)
    {
        var res = await context.procedure
        .AsNoTracking()
        .Where(x => x.device_id == device && x.updated_date > sync)
        .CountAsync();

        return res;
    }

    public async Task<IEnumerable<ModeDto>> GetActionTypeAsync()
    {
        var dtos = await context.action_type
            .AsNoTracking()
            .Select(x => new ModeDto(x.name,x.value,x.description))
            .ToArrayAsync();

        return dtos;
    }

    public async Task<IEnumerable<ProcedureDto>> GetAsync()
    {
        var dtos = await context.procedure
           .AsNoTracking()
           .Select(x => new ProcedureDto(
            x.id,
            x.device_id,
            x.driver_id,
            x.name,
            x.trigger_id,
            x.actions.Select(a => new ActionDto(
                a.id,
                a.device_id,
                a.action_type,
                a.action_type_detail,
                a.arg1,
                a.arg2,
                a.arg3,
                a.arg4,
                a.arg5,
                a.arg6,
                a.arg7,
                a.str_arg,
                a.delay_time,
                a.procedure_id,
                a.location_id,
                a.is_active
                )).ToList(),
            x.location_id,
            x.is_active))
           .ToArrayAsync();

        return dtos;
    }

    public Task<ProcedureDto> GetByComponentIdAsync(short componentId)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<ProcedureDto>> GetByLocationIdAsync(int locationId)
    {
        var dtos = await context.procedure
            .AsNoTracking()
            .Where(x => x.location_id == locationId || x.location_id == 1)
            .Select(x => new ProcedureDto(
            x.id,
            x.device_id,
            x.driver_id,
            x.name,
            x.trigger_id,
            x.actions.Select(a => new ActionDto(
                a.id,
                a.device_id,
                a.action_type,
                a.action_type_detail,
                a.arg1,
                a.arg2,
                a.arg3,
                a.arg4,
                a.arg5,
                a.arg6,
                a.arg7,
                a.str_arg,
                a.delay_time,
                a.procedure_id,
                a.location_id,
                a.is_active
                )).ToList(),
            x.location_id,
            x.is_active))
            .ToArrayAsync();

        return dtos;
    }

    public async Task<short> GetLowestUnassignedNumberAsync(int max, int device)
    {
        if (max <= 0) return -1;

        var query = context.procedure
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

    public async Task<Pagination<ProcedureDto>> GetPaginationAsync(PaginationParamsWithFilter param, int location)
    {

        var query = context.procedure.AsNoTracking().AsQueryable();


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
            .Select(x => new ProcedureDto(
            x.id,
            x.device_id,
            x.driver_id,
            x.name,
            x.trigger_id,
            x.actions.Select(a => new ActionDto(
                a.id,
                a.device_id,
                a.action_type,
                a.action_type_detail,
                a.arg1,
                a.arg2,
                a.arg3,
                a.arg4,
                a.arg5,
                a.arg6,
                a.arg7,
                a.str_arg,
                a.delay_time,
                a.procedure_id,
                a.location_id,
                a.is_active
                )).ToList(),
            x.location_id,
            x.is_active))
            .ToListAsync();


        return new Pagination<ProcedureDto>
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

    public Task<bool> IsAnyByIdAasync(int component)
    {
        throw new NotImplementedException();
    }


      public Task<bool> IsAnyByIdAsync(int id)
      {
            throw new NotImplementedException();
      }

      public Task<ProcedureDto> GetByIdAsync(int id)
      {
            throw new NotImplementedException();
      }


      public Task<bool> IsAnyByNameAsync(string name)
      {
            throw new NotImplementedException();
      }
}
