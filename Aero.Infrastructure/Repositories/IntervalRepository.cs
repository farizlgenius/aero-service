using Aero.Application.DTOs;
using Aero.Domain.Entities;
using Aero.Domain.Interface;
using Aero.Infrastructure.Persistences;
using Aero.Infrastructure.Mapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Cryptography.X509Certificates;
using Aero.Application.Interface;

namespace Aero.Infrastructure.Repositories;

public class IntervalRepository(AppDbContext context) : IIntervalRepository
{
      public async Task<int> AddAsync(Interval data)
      {
            var en = new Aero.Infrastructure.Persistences.Entities.Interval(data.Days,data.DaysDetail,data.StartTime,data.EndTime,data.LocationId);
           await context.interval.AddAsync(en); 
           var record = await context.SaveChangesAsync();
            if (record <= 0) return -1;
            return en.id;
      }

   

      public async Task<int> DeleteByIdAsync(int id)
      {
            var en = await context.interval
            .Include(x => x.timezone_intervals)
            .Where(x => x.id == id)
            .OrderBy(x => x.id)
            .FirstOrDefaultAsync();

            if(en is null) return 0;

            context.timezone_interval.RemoveRange(en.timezone_intervals);
            context.interval.Remove(en);
            return await context.SaveChangesAsync();
      }

      public async Task<int> UpdateAsync(Interval data)
      {
            var en = await context.interval
            .Include(x => x.days)
            .Where(x => x.id == data.Id)
            .OrderBy(x => x.id)
            .FirstOrDefaultAsync();

            if(en is null) return 0;

            en.Update(data);

            context.interval.Update(en);
            return await context.SaveChangesAsync();
            
      }
    public async Task<IEnumerable<IntervalDto>> GetAsync()
    {
        var res = await context.interval
        .AsNoTracking()
        .Select(i => new IntervalDto(
            i.id,
            new DaysInWeekDto(
                i.days.sunday,
                i.days.monday,
                i.days.tuesday,
                i.days.wednesday,
                i.days.thursday,
                i.days.friday,
                i.days.saturday
                ),
            i.days_detail,
            i.start_time,
            i.end_time,
            i.location_id,
            i.is_active
            ))
        .ToArrayAsync();

        return res;
    }

    public async Task<IntervalDto> GetByIdAsync(int id)
    {
        var res = await context.interval
        .AsNoTracking()
        .Where(x => x.id == id)
        .OrderBy(x => x.id)
        .Select(i => new IntervalDto(
            i.id,
            new DaysInWeekDto(
                i.days.sunday,
                i.days.monday,
                i.days.tuesday,
                i.days.wednesday,
                i.days.thursday,
                i.days.friday,
                i.days.saturday
                ),
            i.days_detail,
            i.start_time,
            i.end_time,
            i.location_id,
            i.is_active
            ))
        .FirstOrDefaultAsync();

        return res;
    }

    public async Task<IEnumerable<IntervalDto>> GetByLocationIdAsync(int locationId)
    {
        var res = await context.interval
        .AsNoTracking()
        .Where(x => x.location_id == locationId || x.location_id == 1)
        .OrderBy(x => x.id)
        .Select(i => new IntervalDto(
            i.id,
            new DaysInWeekDto(
                i.days.sunday,
                i.days.monday,
                i.days.tuesday,
                i.days.wednesday,
                i.days.thursday,
                i.days.friday,
                i.days.saturday
                ),
            i.days_detail,
            i.start_time,
            i.end_time,
            i.location_id,
            i.is_active
            ))
        .ToArrayAsync();

        return res;
    }

    public async Task<IEnumerable<IntervalDto>> GetIntervalFromTimezoneIdAsync(int id)
    {
        return await context.interval.AsNoTracking()
        .Where(x => x.timezone_intervals.Any(x => x.timezone_id == id))
        .OrderBy(x => x.id)
        .Select(i => new IntervalDto(
            i.id,
            new DaysInWeekDto(
                i.days.sunday,
                i.days.monday,
                i.days.tuesday,
                i.days.wednesday,
                i.days.thursday,
                i.days.friday,
                i.days.saturday
                ),
            i.days_detail,
            i.start_time,
            i.end_time,
            i.location_id,
            i.is_active
            ))
        .ToArrayAsync();
    }

   

    public async Task<IEnumerable<int>> GetTimezoneIntervalIdByIntervalIdAsync(int id)
    {
        return await context.interval
        .AsNoTracking()
        .Where(x => x.id == id)
        .SelectMany(x => x.timezone_intervals.Select(x => x.timezone_id).ToArray())
        .ToArrayAsync();

    }

    public async Task<bool> IsAnyById(int id)
    {
        return await context.interval.AsNoTracking().AnyAsync(x => x.id == id);
    }

    public async Task<bool> IsAnyOnEachDays(CreateIntervalDto dto)
    {
        return await context.interval.AnyAsync(p =>
        p.start_time == dto.Start &&
        p.end_time == dto.End &&
        p.days.sunday == dto.Days.Sunday &&
        p.days.monday == dto.Days.Monday &&
        p.days.tuesday == dto.Days.Tuesday &&
        p.days.wednesday == dto.Days.Wednesday &&
        p.days.thursday == dto.Days.Thursday &&
        p.days.friday == dto.Days.Friday &&
        p.days.saturday == dto.Days.Saturday
        );
    }

    public async Task<bool> IsAnyReferenceByIdAsync(int id)
    {
        return await context.interval
           .AsNoTracking()
           .AnyAsync(x => x.id == id && x.timezone_intervals.Any());
    }

    public async Task<int> CountByLocationIdAndUpdateTimeAsync(short locationId, DateTime sync)
    {
        var res = await context.interval
        .AsNoTracking()
        .Where(x => x.location_id == locationId && x.updated_date < sync)
        .CountAsync();

        return res;
    }

    public async Task<Pagination<IntervalDto>> GetPaginationAsync(PaginationParamsWithFilter param, int location)
    {

        var query = context.interval.AsNoTracking().AsQueryable();


        if (!string.IsNullOrWhiteSpace(param.Search))
        {
            if (!string.IsNullOrWhiteSpace(param.Search))
            {
                var search = param.Search.Trim();

                if (context.Database.IsNpgsql())
                {
                    var pattern = $"%{search}%";

                    query = query.Where(x =>
                        EF.Functions.ILike(x.days_detail, pattern) ||
                        EF.Functions.ILike(x.start_time, pattern) ||
                        EF.Functions.ILike(x.end_time, pattern)

                    );
                }
                else // SQL Server
                {
                    query = query.Where(x =>
                        x.days_detail.Contains(search) ||
                        x.start_time.Contains(search) ||
                        x.end_time.Contains(search)
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
            .Select(i => new IntervalDto(
                i.id,
            new DaysInWeekDto(
                i.days.sunday,
                i.days.monday,
                i.days.tuesday,
                i.days.wednesday,
                i.days.thursday,
                i.days.friday,
                i.days.saturday
                ),
            i.days_detail,
            i.start_time,
            i.end_time,
            i.location_id,
            i.is_active
            ))
            .ToListAsync();


        return new Pagination<IntervalDto>
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


    public async Task<bool> IsAnyByNameAsync(string name)
    {
        throw new NotImplementedException();
    }


}
