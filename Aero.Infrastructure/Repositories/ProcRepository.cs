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
            var en = ProcedureMapper.ToEf(data);
            await context.procedure.AddAsync(en);
            return await context.SaveChangesAsync();
      }

      public async Task<int> DeleteByComponentIdAsync(short component)
      {
            var en = await context.procedure
            .Where(x => x.component_id == component)
            .OrderBy(x => x.component_id)
            .FirstOrDefaultAsync();

            if(en is null) return 0;

            context.procedure.Remove(en);
            return await context.SaveChangesAsync();
      }

      public async Task<int> UpdateAsync(Procedure newData)
      {
            var en = await context.procedure
            .Include(x => x.actions)
            .Where(x => x.component_id == newData.ComponentId)
            .OrderBy(x => x.component_id)
            .FirstOrDefaultAsync();

            if(en is null) return 0;

            context.action.RemoveRange(en.actions);

            ProcedureMapper.Update(newData,en);

            context.procedure.Update(en);
            return await context.SaveChangesAsync();


      }

    public async Task<int> CountByMacAndUpdateTimeAsync(string mac, DateTime sync)
    {
        var res = await context.procedure
        .AsNoTracking()
        .Where(x => x.trigger.hardware_mac.Equals(mac) && x.updated_date > sync)
        .CountAsync();

        return res;
    }

    public async Task<IEnumerable<Mode>> GetActionTypeAsync()
    {
        var dtos = await context.action_type
            .AsNoTracking()
            .Select(x => new Mode
            {
                Name = x.name,
                Value = x.value,
                Description = x.description,
            }).ToArrayAsync();

        return dtos;
    }

    public async Task<IEnumerable<ProcedureDto>> GetAsync()
    {
        var dtos = await context.procedure
           .AsNoTracking()
           .Select(x => new ProcedureDto
           {
               // Base
               ComponentId = x.component_id,
               Mac = x.trigger.hardware_mac,
               HardwareName = x.trigger.hardware.name,
               LocationId = x.location_id,
               IsActive = x.is_active,

               // Detail
               Name = x.name,
               Actions = x.actions
           .Select(en => new ActionDto
           {
               // Base
               ComponentId = en.component_id,
               Mac = x.trigger.hardware_mac,
               LocationId = en.location_id,
               IsActive = en.is_active,

               // Detail
               ScpId = en.scp_id,
               ActionType = en.action_type,
               ActionTypeDesc = en.action_type_detail,
               Arg1 = en.arg1,
               Arg2 = en.arg2,
               Arg3 = en.arg3,
               Arg4 = en.arg4,
               Arg5 = en.arg5,
               Arg6 = en.arg6,
               Arg7 = en.arg7,
               StrArg = en.str_arg,
               DelayTime = en.delay_time,
           })
           .ToList()


           })
           .ToArrayAsync();

        return dtos;
    }

    public Task<ProcedureDto> GetByComponentIdAsync(short componentId)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<ProcedureDto>> GetByLocationIdAsync(short locationId)
    {
        var dtos = await context.procedure
            .AsNoTracking()
            .Where(x => x.location_id == locationId || x.location_id == 1)
            .Select(x => new ProcedureDto
            {
                // Base
                ComponentId = x.component_id,
                Mac = x.trigger.hardware_mac,
                HardwareName = x.trigger.hardware.name,
                LocationId = x.location_id,
                IsActive = x.is_active,

                // Detail
                Name = x.name,
                Actions = x.actions
            .Select(en => new ActionDto
            {
                // Base
                ComponentId = en.component_id,
                Mac = x.trigger.hardware_mac,
                LocationId = en.location_id,
                IsActive = en.is_active,

                // Detail
                ScpId = en.scp_id,
                ActionType = en.action_type,
                ActionTypeDesc = en.action_type_detail,
                Arg1 = en.arg1,
                Arg2 = en.arg2,
                Arg3 = en.arg3,
                Arg4 = en.arg4,
                Arg5 = en.arg5,
                Arg6 = en.arg6,
                Arg7 = en.arg7,
                StrArg = en.str_arg,
                DelayTime = en.delay_time,
            })
            .ToList()


            })
            .ToArrayAsync();

        return dtos;
    }

    public async Task<short> GetLowestUnassignedNumberAsync(int max, string mac)
    {
        if (max <= 0) return -1;

        var query = context.procedure
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

    public async Task<Pagination<ProcedureDto>> GetPaginationAsync(PaginationParamsWithFilter param, short location)
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
            .Select(x => new ProcedureDto
            {
                // Base
                ComponentId = x.component_id,
                Mac = x.trigger.hardware_mac,
                HardwareName = x.trigger.hardware.name,
                LocationId = x.location_id,
                IsActive = x.is_active,

                // Detail
                Name = x.name,
                Actions = x.actions
               .Select(en => new ActionDto
               {
                   // Base
                   ComponentId = en.component_id,
                   Mac = x.trigger.hardware_mac,
                   LocationId = en.location_id,
                   IsActive = en.is_active,

                   // Detail
                   ScpId = en.scp_id,
                   ActionType = en.action_type,
                   ActionTypeDesc = en.action_type_detail,
                   Arg1 = en.arg1,
                   Arg2 = en.arg2,
                   Arg3 = en.arg3,
                   Arg4 = en.arg4,
                   Arg5 = en.arg5,
                   Arg6 = en.arg6,
                   Arg7 = en.arg7,
                   StrArg = en.str_arg,
                   DelayTime = en.delay_time,
               })
               .ToList()


            })
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

    public Task<bool> IsAnyByComponentId(short component)
    {
        throw new NotImplementedException();
    }
}
