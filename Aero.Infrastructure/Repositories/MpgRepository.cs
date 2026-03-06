using Aero.Application.DTOs;
using Aero.Domain.Entities;
using Aero.Domain.Interface;
using Aero.Infrastructure.Persistences;
using Aero.Infrastructure.Mapper;
using Microsoft.EntityFrameworkCore;
using System;
using Aero.Application.Interface;

namespace Aero.Infrastructure.Repositories;

public class MpgRepository(AppDbContext context) : IMpgRepository
{
      public async Task<int> AddAsync(MonitorGroup data)
      {
            var en = new Aero.Infrastructure.Persistences.Entities.MonitorGroup(data);
            await context.monitor_group.AddAsync(en);
            var rec = await context.SaveChangesAsync();
            if(rec <= 0) return -1;
            return en.id;
      }


      public async Task<int> DeleteByIdAsync(int id)
      {
            var en = await context.monitor_group
            .Where(x => x.id == id)
            .FirstOrDefaultAsync();

            if(en is null) return 0;

            context.monitor_group.Remove(en);
            return await context.SaveChangesAsync();
      }

    //   public async Task<int> DeleteByMacAndIdAsync(string mac, int id)
    //   {
    //         var en = await context.monitor_group
    //         .Where(x => x.mac.Equals(mac) && x.component_id == component)
    //         .FirstOrDefaultAsync();

    //         if(en is null) return 0;

    //         context.monitor_group.Remove(en);
    //         return await context.SaveChangesAsync();
    //   }

      public async Task<int> DeleteReferenceByIdAsync(int Id)
      {
            var en = await context.monitor_group_list.Where(x => x.monitor_group_id == Id).ToArrayAsync();

            context.monitor_group_list.RemoveRange(en);
            return await context.SaveChangesAsync();
      }

      public async Task<int> UpdateAsync(MonitorGroup data)
      {
            var en = await context.monitor_group
            .Where(x => x.id == data.Id)
            .FirstOrDefaultAsync();

            if(en is null) return 0;

            en.Update(data);

            context.monitor_group.Update(en);
            return await context.SaveChangesAsync();
      }

    public async Task<int> CountByDriverIdAndUpdateTimeAsync(int driverid, DateTime sync)
    {
        var res = await context.monitor_group
        .AsNoTracking()
        .Where(x => x.driver_id == driverid && x.updated_date > sync)
        .CountAsync();

        return res;
    }

    public async Task<IEnumerable<MonitorGroupDto>> GetAsync()
    {
        var res = await context.monitor_group
        .AsNoTracking()
        .Select(x => new MonitorGroupDto(
            x.id,
            x.device_id,
            x.driver_id,
            x.name,
            x.n_mp_count,
            x.n_mp_list.Select(x => new MonitorGroupListDto(x.point_type,x.point_type_detail,x.point_number)).ToList(),
            x.location_id,
            x.is_active))
            .ToArrayAsync();

        return res;
    }

    public async Task<MonitorGroupDto> GetByIdAsync(int id)
    {

        var res = await context.monitor_group
        .AsNoTracking()
        .OrderBy(x => x.id)
        .Where(x => x.id == id)
        .Select(x => new MonitorGroupDto(
            x.id,
            x.device_id,
            x.driver_id,
            x.name,
            x.n_mp_count,
            x.n_mp_list.Select(x => new MonitorGroupListDto(x.point_type,x.point_type_detail,x.point_number)).ToList(),
            x.location_id,
            x.is_active))
            .FirstOrDefaultAsync();

        return res;
    }

    public async Task<IEnumerable<MonitorGroupDto>> GetByLocationIdAsync(int locationId)
    {
        var res = await context.monitor_group
        .AsNoTracking()
        .OrderBy(x => x.id)
        .Where(x => x.location_id == locationId || x.location_id == 1)
        .Select(x => new MonitorGroupDto(
            x.id,
            x.device_id,
            x.driver_id,
            x.name,
            x.n_mp_count,
            x.n_mp_list.Select(x => new MonitorGroupListDto(x.point_type,x.point_type_detail,x.point_number)).ToList(),
            x.location_id,
            x.is_active))
            .ToArrayAsync();

        return res;
    }

    // public async Task<IEnumerable<MonitorGroupDto>> GetByMacAsync(string mac)
    // {
    //     var res = await context.monitor_group
    //    .AsNoTracking()
    //    .OrderBy(x => x.id)
    //    .Where(x => x.mac.Equals(mac))
    //     .Select(x => new MonitorGroupDto(
    //         x.id,
    //         x.name,
    //         x.n_mp_count,
    //         x.n_mp_list.Select(x => new MonitorGroupListDto(x.point_type,x.point_type_detail,x.point_number)).ToList(),
    //         x.location_id,
    //         x.is_active))
    //        .ToArrayAsync();

    //     return res;
    // }

    public async Task<IEnumerable<ModeDto>> GetCommandAsync()
    {
        var dtos = await context.monitor_group_command
             .AsNoTracking()
             .Select(x => new ModeDto(x.name,x.value,x.description))
             .ToArrayAsync();

        return dtos;
    }

    public async Task<short> GetLowestUnassignedNumberAsync(int max, int device)
    {
        if (max <= 0) return -1;

        var query = context.monitor_group
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

    public async Task<IEnumerable<ModeDto>> GetTypeAsync()
    {
        var dtos = await context.monitor_group_type
             .AsNoTracking()
             .Select(x => new ModeDto(x.name,x.value,x.description))
             .ToArrayAsync();

        return dtos;
    }

    public async Task<Pagination<MonitorGroupDto>> GetPaginationAsync(PaginationParamsWithFilter param, int location)
    {

        var query = context.monitor_group.AsNoTracking().AsQueryable();


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
          .Select(x => new MonitorGroupDto(
            x.id,
            x.device_id,
            x.driver_id,
            x.name,
            x.n_mp_count,
            x.n_mp_list.Select(x => new MonitorGroupListDto(x.point_type,x.point_type_detail,x.point_number)).ToList(),
            x.location_id,
            x.is_active))
            .ToListAsync();


        return new Pagination<MonitorGroupDto>
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
        return await context.location.AnyAsync(x => x.id == id);
    }

      public async Task<bool> IsAnyByNameAsync(string name)
      {
            return await context.location.AnyAsync(x => x.name == name);
      }

      public async Task<IEnumerable<MonitorGroupDto>> GetByDeviceIdAsync(int device)
      {
            var res = await context.monitor_group
            .AsNoTracking()
            .Where(x => x.device_id == device)
            .Select(x => new MonitorGroupDto(
                 x.id,
            x.device_id,
            x.driver_id,
            x.name,
            x.n_mp_count,
            x.n_mp_list.Select(x => new MonitorGroupListDto(x.point_type,x.point_type_detail,x.point_number)).ToList(),
            x.location_id,
            x.is_active
            ))
            .ToArrayAsync();

            return res;
            
      }
}
