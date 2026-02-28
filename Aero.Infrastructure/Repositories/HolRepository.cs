using Aero.Application.DTOs;
using Aero.Domain.Entities;
using Aero.Domain.Interface;
using Aero.Infrastructure.Persistences;
using Aero.Infrastructure.Mapper;
using Microsoft.EntityFrameworkCore;
using System;
using Aero.Application.Interface;

namespace Aero.Infrastructure.Repositories;

public class HolRepository(AppDbContext context) : IHolRepository
{
      public async Task<int> AddAsync(Holiday data)
      {
            var en = new Aero.Infrastructure.Persistences.Entities.Holiday(data.DriverId,data.Name,data.Year,data.Month,data.Day,data.Extend,data.TypeMask,data.LocationId);
            await context.holiday.AddAsync(en);
            var record = await context.SaveChangesAsync();
        if (record <= 0) return -1;
        return en.id;
      }

      public async Task<int> DeleteByIdAsync(int id)
      {
            var en = await context.holiday
            .Where(x => x.id == id)
            .OrderBy(x => x.id)
            .FirstOrDefaultAsync();

            if(en is null) return 0;

            context.holiday.Remove(en);
            return await context.SaveChangesAsync();
      }

      //public async Task<Holiday> GetByIdAsync(int id)
      //{
      //      var res = await context.holiday.AsNoTracking()
      //      .Where(x => x.id == id)
      //      .OrderBy(x => x.id)
      //      .Select(h => new HolidayDto())
      //      .FirstOrDefaultAsync();

      //      return res;
      //}

      public async Task<int> RemoveAllAsync()
      {
            var en = await context.holiday.ToArrayAsync();
            context.holiday.RemoveRange(en);
            return await context.SaveChangesAsync();
      }

      public async Task<int> UpdateAsync(Holiday data)
      {
            var en = await context.holiday
            .Where(x => x.id == data.Id)
            .OrderBy(x => x.id)
            .FirstOrDefaultAsync();

            if(en is null) return 0;

            en.Update(data);

            context.holiday.Update(en);
            return await context.SaveChangesAsync();
      }

    public async Task<int> CountByLocationIdAndUpdateTimeAsync(int locationId, DateTime sync)
    {
        var res = await context.holiday
        .AsNoTracking()
        .Where(x => x.location_id == locationId && x.updated_date < sync)
        .CountAsync();

        return res;
    }

    public async Task<IEnumerable<HolidayDto>> GetAsync()
    {
        var res = await context.holiday
        .AsNoTracking()
        .Select(h => new HolidayDto(h.id,h.driver_id,h.name,h.year,h.month,h.day,h.extend,h.type_mask,h.location_id,h.is_active))
        .ToArrayAsync();

        return res;
    }

    public async Task<HolidayDto> GetByIdAsync(int id)
    {
        var res = await context.holiday
        .AsNoTracking()
        .Where(x => x.id == id)
        .Select(h => new HolidayDto(h.id, h.driver_id, h.name, h.year, h.month, h.day, h.extend, h.type_mask, h.location_id, h.is_active))
        .FirstOrDefaultAsync();

        return res;
    }

    public async Task<IEnumerable<HolidayDto>> GetByLocationIdAsync(int locationId)
    {
        var res = await context.holiday
        .AsNoTracking()
        .Where(p => p.location_id == locationId || p.location_id == locationId)
        .Select(h => new HolidayDto(h.id, h.driver_id, h.name, h.year, h.month, h.day, h.extend, h.type_mask, h.location_id, h.is_active)).ToArrayAsync();

        return res;
    }

    public async Task<short> GetLowestUnassignedNumberAsync(int max)
    {
        if (max <= 0) return -1;

        var query = context.holiday
            .AsNoTracking()
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

    public async Task<Pagination<HolidayDto>> GetPaginationAsync(PaginationParamsWithFilter param, int location)
    {

        var query = context.holiday.AsNoTracking().AsQueryable();


        if (!string.IsNullOrWhiteSpace(param.Search))
        {
            if (!string.IsNullOrWhiteSpace(param.Search))
            {
                var search = param.Search.Trim();

                if (context.Database.IsNpgsql())
                {
                    var pattern = $"%{search}%";

                    query = query.Where(x =>
                        EF.Functions.ILike(x.year.ToString(), pattern) ||
                        EF.Functions.ILike(x.month.ToString(), pattern) ||
                        EF.Functions.ILike(x.day.ToString(), pattern)

                    );
                }
                else // SQL Server
                {
                    query = query.Where(x =>
                        x.year.ToString().Contains(search) ||
                        x.month.ToString().Contains(search) ||
                        x.day.ToString().Contains(search)
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
            .Select(h => new HolidayDto(h.id, h.driver_id, h.name, h.year, h.month, h.day, h.extend, h.type_mask, h.location_id, h.is_active))
            .ToListAsync();


        return new Pagination<HolidayDto>
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
        return await context.holiday.AnyAsync(x => x.id == id);
    }

    public async Task<bool> IsAnyWithSameDataAsync(short day, short month, short year)
    {
        return await context.holiday.AnyAsync(u => u.day == day && u.month == month && u.year == year);
    }

    public async Task<bool> IsAnyByNameAsync(string name)
    {
        return await context.holiday.AnyAsync(x => x.name.Equals(name));
    }
}
