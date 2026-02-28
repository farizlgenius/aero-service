using Aero.Application.DTOs;
using Aero.Application.Interface;
using Aero.Domain.Entities;
using Aero.Domain.Interface;
using Aero.Infrastructure.Mapper;
using Aero.Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Cryptography;

namespace Aero.Infrastructure.Repositories;

public class CpRepository(AppDbContext context) : ICpRepository
{
      public async Task<int> AddAsync(ControlPoint data)
      {
            var en = new Aero.Infrastructure.Persistences.Entities.ControlPoint(data);
            await context.control_point.AddAsync(en);
            var rec = await context.SaveChangesAsync();
        if (rec <= 0) return -1;
        return en.id;
      }

      public async Task<int> DeleteByIdAsync(int id)
      {
            var en = await context.control_point
            .Where(x => x.id == id)
            .FirstOrDefaultAsync();

            if(en is null) return 0;

            context.control_point.Remove(en);
            return await context.SaveChangesAsync();
      }

      public async Task<int> UpdateAsync(ControlPoint data)
      {
            var en = await context.control_point
            .Where(x => x.id == data.Id)
            .FirstOrDefaultAsync();

            if(en is null) return 0;

            en.Update(data);

            context.control_point.Update(en);
            return await context.SaveChangesAsync();
      }

    public async Task<int> CountByMacAndUpdateTimeAsync(string mac, DateTime sync)
    {
        var res = await context.control_point
        .AsNoTracking()
        .Where(x => x.module.device_id.Equals(mac) && x.updated_date > sync)
        .CountAsync();

        return res;
    }

    public async Task<IEnumerable<ControlPointDto>> GetAsync()
    {
        var res = await context.control_point
        .AsNoTracking()
        .OrderBy(x => x.id)
        .Select(x=> new ControlPointDto(x.id,x.driver_id,x.name,x.module_id,x.module_detail,x.output_no,x.relaymode,x.relaymode_detail,x.offlinemode,x.offlinemode_detail,x.default_pulse,x.device_id,x.location_id,x.is_active))
        .ToArrayAsync();

        return res;
    }

    public async Task<IEnumerable<short>> GetAvailableOpAsync(int DeviceId, short ModuleId)
    {
        var ops = await context.module
            .AsNoTracking()
            .Where(sio => sio.device_id == DeviceId && sio.driver_id == ModuleId)
            .Select(cp => cp.n_output)
            .FirstOrDefaultAsync();

        var strk = await context.strike
            .AsNoTracking()
            .Where(x => x.module_id == ModuleId && x.module.device_id == DeviceId)
            .Select(x => x.output_no)
            .ToArrayAsync();

        var cp = await context.control_point
            .AsNoTracking()
            .Where(x => x.module_id == ModuleId && x.module.device_id == DeviceId)
            .Select(x => x.output_no)
            .ToArrayAsync();


        var unavailable = strk
            .Union(cp)
            .Distinct()
            .ToList();

        List<short> all = Enumerable.Range(0, ops).Select(x => (short)x).ToList();
        return all.Except(unavailable).ToList();
    }

    public async Task<ControlPointDto> GetByIdAsync(int id)
    {
        var res = await context.control_point
        .AsNoTracking()
        .Where(x => x.id == id)
        .OrderBy(x => x.id)
       .Select(x => new ControlPointDto(x.id, x.driver_id, x.name, x.module_id, x.module_detail, x.output_no, x.relaymode, x.relaymode_detail, x.offlinemode, x.offlinemode_detail, x.default_pulse, x.device_id, x.location_id, x.is_active))
        .FirstOrDefaultAsync();

        return res;
    }

    public async Task<IEnumerable<ControlPointDto>> GetByLocationIdAsync(int locationId)
    {
        var res = await context.control_point
        .AsNoTracking()
        .Where(x => x.location_id == locationId || x.location_id == 1)
        .OrderBy(x => x.id)
        .Select(x => new ControlPointDto(x.id, x.driver_id, x.name, x.module_id, x.module_detail, x.output_no, x.relaymode, x.relaymode_detail, x.offlinemode, x.offlinemode_detail, x.default_pulse, x.device_id, x.location_id, x.is_active))
        .ToArrayAsync();

        return res;
    }

    public async Task<ControlPointDto> GetByDeviceAndIdAsync(int deviceId, int id)
    {
        var dto = await context.control_point
           .Where(x => x.module.device_id == deviceId && x.id == id)
          .Select(x => new ControlPointDto(x.id, x.driver_id, x.name, x.module_id, x.module_detail, x.output_no, x.relaymode, x.relaymode_detail, x.offlinemode, x.offlinemode_detail, x.default_pulse, x.device_id, x.location_id, x.is_active))
          .FirstOrDefaultAsync();

        return dto;


    }

    public async Task<IEnumerable<ControlPointDto>> GetByDeviceId(int device)
    {
        var res = await context.control_point
        .AsNoTracking()
        .OrderBy(x => x.id)
        .Where(x => x.device_id == device)
         .Select(x => new ControlPointDto(x.id, x.driver_id, x.name, x.module_id, x.module_detail, x.output_no, x.relaymode, x.relaymode_detail, x.offlinemode, x.offlinemode_detail, x.default_pulse, x.device_id, x.location_id, x.is_active))
        .ToArrayAsync();

        return res;
    }

    public async Task<short> GetLowestUnassignedNumberAsync(int max, int device_id)
    {
        if (max <= 0) return -1;

        var query = context.control_point
            .AsNoTracking()
            .Where(x => x.device_id == device_id)
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

    public async Task<short> GetModeNoByOfflineAndRelayModeAsync(short offlineMode, short relayMode)
    {
        var res = await context.output_mode
        .AsNoTracking()
        .OrderBy(x => x.id)
        .Where(x => x.offline_mode == offlineMode && x.relay_mode == relayMode)
        .Select(x => x.value)
        .FirstOrDefaultAsync();

        return res;
    }

    public async Task<IEnumerable<ModeDto>> GetOfflineModeAsync()
    {
        var dtos = await context.relay_offline_mode.AsNoTracking()
            .Select(x => new ModeDto(x.name,x.value,x.description))
            .ToArrayAsync();

        return dtos;
    }

    public async Task<IEnumerable<ModeDto>> GetRelayModeAsync()
    {
        var dtos = await context.relay_mode.AsNoTracking().Select(x => new ModeDto(x.name, x.value, x.description)).ToArrayAsync();
        return dtos;
    }

    public async Task<Pagination<ControlPointDto>> GetPaginationAsync(PaginationParamsWithFilter param, int location)
    {

        var query = context.control_point.AsNoTracking().AsQueryable();


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
                        EF.Functions.ILike(x.module_detail, pattern) ||
                        EF.Functions.ILike(x.relaymode_detail, pattern) ||
                        EF.Functions.ILike(x.offlinemode_detail, pattern)

                    );
                }
                else // SQL Server
                {
                    query = query.Where(x =>
                        x.name.Contains(search) ||
                        x.module_detail.Contains(search) ||
                        x.relaymode_detail.Contains(search) ||
                        x.offlinemode_detail.Contains(search)
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
             .Select(x => new ControlPointDto(x.id, x.driver_id, x.name, x.module_id, x.module_detail, x.output_no, x.relaymode, x.relaymode_detail, x.offlinemode, x.offlinemode_detail, x.default_pulse, x.device_id, x.location_id, x.is_active))
            .ToArrayAsync();


        return new Pagination<ControlPointDto>
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

    public async Task<bool> IsAnyById(int id)
    {
        return await context.control_point.AsNoTracking().AnyAsync(x => x.id == id);
    }

    public async Task<bool> IsAnyByNameAsync(string name)
    {
        return await context.control_point.AnyAsync(x => x.name.Equals(name));
    }
}
