using Aero.Application.DTOs;
using Aero.Domain.Entities;
using Aero.Domain.Interface;
using Aero.Infrastructure.Persistences;
using Aero.Infrastructure.Mapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO.Compression;

namespace Aero.Infrastructure.Repositories;

public sealed class MpRepository(AppDbContext context) : IMpRepository
{
      public async Task<int> AddAsync(MonitorPoint data)
      {
            var en = new Persistences.Entities.MonitorPoint(data);

            await context.monitor_point.AddAsync(en);
            var rec = await context.SaveChangesAsync();
            if(rec <= 0) return -1;
            return en.id;
      }

      public async Task<int> DeleteByIdAsync(int id)
      {
            var en = await context.monitor_point
            .Where(x => x.id == id)
            .OrderBy(x => x.id)
            .FirstOrDefaultAsync();

            if(en is null) return 0;

            context.monitor_point.Remove(en);
            return await context.SaveChangesAsync();
      }

      public async Task<int> SetMaskByIdAsync(int id,bool mask)
      {
            var en = await context.monitor_point
            .Where(x => x.id == id)
            .OrderBy(x => x.id)
            .FirstOrDefaultAsync();

            if(en is null) return 0;

            en.SetMask(mask);
            context.monitor_point.Update(en);
            return await context.SaveChangesAsync();
      }

      public async Task<int> UpdateAsync(MonitorPoint data)
      {
            var en = await context.monitor_point
            .Where(x => x.id == data.Id)
            .OrderBy(x => x.id)
            .FirstOrDefaultAsync();

            if(en is null) return 0;

            en.Update(data);

            context.monitor_point.Update(en);
            return await context.SaveChangesAsync();
      }

    public async Task<int> CountByDeviceIdAndUpdateTimeAsync(int deviceId, DateTime sync)
    {
        var res = await context.monitor_point.AsNoTracking()
        .Where(x => x.device_id == deviceId && x.updated_date > sync)
        .CountAsync();

        return res;
    }

    public async Task<IEnumerable<MonitorPointDto>> GetAsync()
    {
        var res = await context.monitor_point
        .AsNoTracking()
        .OrderBy(x => x.id)
        .Select(x => new MonitorPointDto(
            x.id,
            x.device_id,
            x.driver_id,
            x.name,
            (short)x.module_id,
            x.module.model_detail,
            x.input_no,
            x.input_mode,
            x.input_mode_detail,
            x.debounce,
            x.holdtime,
            x.log_function,
            x.log_function_detail,
            x.monitor_point_mode,
            x.monitor_point_mode_detail,
            x.delay_entry,
            x.delay_exit,
            x.is_mask,
            x.location_id,
            x.is_active
        ))
        .ToArrayAsync();

        return res;
    }

    public async Task<IEnumerable<short>> GetAvailableIpAsync(int moduleId)
    {
        var input = await context.module
            .AsNoTracking()
            .Where(mp => mp.id == moduleId)
            .Select(mp => mp.n_input)
            .FirstOrDefaultAsync();

        var sensors = await context.sensor
            .AsNoTracking()
            .Where(x => x.module_id == moduleId)
            .Select(x => x.input_no)
            .ToArrayAsync();

        var rex = await context.request_exit
            .AsNoTracking()
            .Where(x => x.module_id == moduleId)
            .Select(x => x.input_no)
            .ToArrayAsync();

        var mp = await context.monitor_point
            .AsNoTracking()
            .Where(x => x.module_id == moduleId)
            .Select(x => x.input_no)
            .ToArrayAsync();


        var unavailable = sensors
            .Union(rex)
            .Union(mp)
            .Distinct()
            .ToList();


        List<short> all = Enumerable.Range(0, input - 3).Select(i => (short)i).ToList();
        IEnumerable<short> av = all.Except(unavailable).ToArray();
        return av;
    }

    public async Task<MonitorPointDto> GetByIdAsync(int id)
    {
        var res = await context.monitor_point
        .AsNoTracking()
        .Where(x => x.id == id)
        .OrderBy(x => x.id)
       .Select(x => new MonitorPointDto(
            x.id,
            x.device_id,
            x.driver_id,
            x.name,
            (short)x.module_id,
            x.module.model_detail,
            x.input_no,
            x.input_mode,
            x.input_mode_detail,
            x.debounce,
            x.holdtime,
            x.log_function,
            x.log_function_detail,
            x.monitor_point_mode,
            x.monitor_point_mode_detail,
            x.delay_entry,
            x.delay_exit,
            x.is_mask,
            x.location_id,
            x.is_active
        ))
        .FirstOrDefaultAsync();

        return res;
    }

    public async Task<IEnumerable<MonitorPointDto>> GetByLocationIdAsync(int locationId)
    {
        var res = await context.monitor_point
        .AsNoTracking()
        .Where(x => x.location_id == locationId || x.location_id == 1)
        .OrderBy(x => x.id)
        .Select(x => new MonitorPointDto(
            x.id,
            x.device_id,
            x.driver_id,
            x.name,
            (short)x.module_id,
            x.module.model_detail,
            x.input_no,
            x.input_mode,
            x.input_mode_detail,
            x.debounce,
            x.holdtime,
            x.log_function,
            x.log_function_detail,
            x.monitor_point_mode,
            x.monitor_point_mode_detail,
            x.delay_entry,
            x.delay_exit,
            x.is_mask,
            x.location_id,
            x.is_active
        ))
        .ToArrayAsync();

        return res;
    }

    public async Task<IEnumerable<MonitorPointDto>> GetByDeviceIdAsync(int id)
    {
        var res = await context.monitor_point
        .AsNoTracking()
        .Where(x => x.id == id)
        .OrderBy(x => x.id)
         .Select(x => new MonitorPointDto(
            x.id,
            x.device_id,
            x.driver_id,
            x.name,
            (short)x.module_id,
            x.module.model_detail,
            x.input_no,
            x.input_mode,
            x.input_mode_detail,
            x.debounce,
            x.holdtime,
            x.log_function,
            x.log_function_detail,
            x.monitor_point_mode,
            x.monitor_point_mode_detail,
            x.delay_entry,
            x.delay_exit,
            x.is_mask,
            x.location_id,
            x.is_active
        ))
        .ToArrayAsync();

        return res;
    }


    public async Task<IEnumerable<ModeDto>> GetInputModeAsync()
    {
        var res = await context.input_mode.AsNoTracking()
        .Select(x => new ModeDto(x.name,x.value,x.description))
        .ToArrayAsync();

        return res;
    }

    public async Task<IEnumerable<ModeDto>> GetLogFunctionAsync()
    {
        var dtos = await context.monitor_point_log_function
            .AsNoTracking()
            .Select(x => new ModeDto(x.name,x.value,x.description))
            .ToArrayAsync();

        return dtos;
    }

    public async Task<short> GetLowestUnassignedNumberAsync(int max, int device)
    {
        if (max <= 0) return -1;

            var query = context.monitor_point
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

    public async Task<int> GetDeviceIdFromDriverIdIdAsync(int driver)
    {
        return await context.monitor_point
        .AsNoTracking()
        .Where(x => x.driver_id == driver)
        .OrderBy(x => x.id)
        .Select(x => x.device_id)
        .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<ModeDto>> GetMonitorPointModeAsync()
    {
        var res = await context.monitor_point_mode.AsNoTracking().Select(x => new ModeDto(x.name,x.value,x.description)).ToArrayAsync();

        return res;
    }

    public async Task<Pagination<MonitorPointDto>> GetPaginationAsync(PaginationParamsWithFilter param, int location)
    {

        var query = context.monitor_point.AsNoTracking().AsQueryable();


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
                        EF.Functions.ILike(x.input_mode_detail, pattern) ||
                        EF.Functions.ILike(x.monitor_point_mode_detail, pattern) 

                    );
                }
                else // SQL Server
                {
                    query = query.Where(x =>
                        x.name.Contains(search) ||
                        x.input_mode_detail.Contains(search) ||
                        x.monitor_point_mode_detail.Contains(search) 
                    );
                }
            }
        }

        query = query.Where(x => x.location_id == location);

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
             .Select(x => new MonitorPointDto(
            x.id,
            x.device_id,
            x.driver_id,
            x.name,
            (short)x.module_id,
            x.module.model_detail,
            x.input_no,
            x.input_mode,
            x.input_mode_detail,
            x.debounce,
            x.holdtime,
            x.log_function,
            x.log_function_detail,
            x.monitor_point_mode,
            x.monitor_point_mode_detail,
            x.delay_entry,
            x.delay_exit,
            x.is_mask,
            x.location_id,
            x.is_active
        ))
            .ToListAsync();


        return new Pagination<MonitorPointDto>
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

    public async Task<bool> IsAnyByIdAsync(int id)
    {
        return await context.monitor_point.AnyAsync(x => x.id == id);
    }

    public async Task<bool> IsAnyByDeviceIdAndDriverIdAsync(int deviceId, short driver)
    {
        return await context.monitor_point.AnyAsync(x => x.device_id == deviceId && x.driver_id == driver);
    }

      public async Task<bool> IsAnyByNameAsync(string name)
      {
           return await context.monitor_point.AsNoTracking().AnyAsync(x => x.name.Equals(name));
      }

      public async Task<IEnumerable<MonitorPointDto>> GetByDeviceId(int device)
      {
            var res = await context.monitor_point
        .AsNoTracking()
        .Where(x => x.device_id == device)
        .OrderBy(x => x.id)
        .Select(x => new MonitorPointDto(
            x.id,
            x.device_id,
            x.driver_id,
            x.name,
            (short)x.module_id,
            x.module.model_detail,
            x.input_no,
            x.input_mode,
            x.input_mode_detail,
            x.debounce,
            x.holdtime,
            x.log_function,
            x.log_function_detail,
            x.monitor_point_mode,
            x.monitor_point_mode_detail,
            x.delay_entry,
            x.delay_exit,
            x.is_mask,
            x.location_id,
            x.is_active
        ))
        .ToArrayAsync();

        return res;
      }
}
