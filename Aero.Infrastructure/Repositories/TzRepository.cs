using Aero.Application.DTOs;
using Aero.Domain.Entities;
using Aero.Domain.Interface;
using Aero.Infrastructure.Persistences;
using Aero.Infrastructure.Mapper;
using Microsoft.EntityFrameworkCore;
using System;
using Aero.Application.Interface;
using Aero.Infrastructure.Persistences.Entities;

namespace Aero.Infrastructure.Repositories;

public class TzRepository(AppDbContext context) : ITzRepository
{


      public async Task<int> AddAsync(Aero.Domain.Entities.TimeZone data)
      {
        var en = new Aero.Infrastructure.Persistences.Entities.TimeZone(data);
        await context.timezone.AddAsync(en);
        var record = await context.SaveChangesAsync();
        if (record <= 0) return -1;
        return en.id;
      }

      public async Task<int> DeleteByIdAsync(int id)
      {
            var en = await context.timezone
            .Where(x => x.id == id)
            .OrderBy(x => x.id)
            .FirstOrDefaultAsync();

            if(en is null) return 0;

            context.timezone.Remove(en);
            return await context.SaveChangesAsync();
      }

      public async Task<int> UpdateAsync(Domain.Entities.TimeZone d)
      {
            var en = await context.timezone
            .Where(x => x.id == d.Id)
            .OrderBy(x => x.id)
            .FirstOrDefaultAsync();

            if(en is null) return 0;

            // Update Process
            en.name = d.Name;
            en.mode = d.Mode;
            en.active_time = d.ActiveTime;
            en.deactive_time = d.DeactiveTime;

            context.timezone.Update(en);

            return await context.SaveChangesAsync();
      }

    public async Task<int> CountByLocationIdAndUpdateTimeAsync(short locationId, DateTime sync)
    {
        var res = await context.timezone
        .AsNoTracking()
        .Where(x => x.location_id == locationId && x.updated_date > sync)
        .CountAsync();

        return res;
    }

    public async Task<TimeZoneDto> GetByLocationAndNameAsync(string name,int locationId)
    {
        var data = await context.timezone
        .AsNoTracking()
        .Where(x => x.name.Equals(name) && x.location_id == locationId)
        .OrderBy(x => x.id)
        .Select(t => new
        {
            t.id,
            t.driver_id,
            t.name,
            t.mode,
            t.active_time,
            t.deactive_time,
            intervals = t.timezone_intervals.Select(i => new
            {
                days = new
                {
                    sun = i.interval.days.sunday,
                    mon = i.interval.days.monday,
                    tue = i.interval.days.tuesday,
                    wed = i.interval.days.wednesday,
                    thu = i.interval.days.thursday,
                    fri = i.interval.days.friday,
                    sat = i.interval.days.saturday
                },
                i.interval.days_detail,
                i.interval.start_time,
                i.interval.end_time,
                i.interval.location_id,
                i.interval.is_active

            }),
            t.location_id,
            t.is_active
        }
        )
        .FirstOrDefaultAsync();

        var res = new TimeZoneDto(
                data.id,
                data.driver_id,
                data.name,
                data.mode,
                data.active_time,
                data.deactive_time,
                data.intervals.Select(
                    i => new IntervalDto(
                        new DaysInWeekDto(
                            i.days.sun,
                            i.days.mon,
                            i.days.tue,
                            i.days.wed,
                            i.days.thu,
                            i.days.fri,
                            i.days.sat
                            ),
                        i.days_detail,
                        i.start_time,
                        i.end_time,
                        i.location_id,
                        i.is_active
                        )).ToList(),
                data.location_id,
                data.is_active
                );

        return res;

    }

    public async Task<IEnumerable<TimeZoneDto>> GetAsync()
    {
        var data = await context.timezone
        .AsNoTracking()
        .Select(t => new
        {
            t.id,
            t.driver_id,
            t.name,
            t.mode,
            t.active_time,
            t.deactive_time,
            intervals = t.timezone_intervals.Select(i => new
            {
                days = new
                {
                    sun = i.interval.days.sunday,
                    mon = i.interval.days.monday,
                    tue = i.interval.days.tuesday,
                    wed = i.interval.days.wednesday,
                    thu = i.interval.days.thursday,
                    fri = i.interval.days.friday,
                    sat = i.interval.days.saturday
                },
                i.interval.days_detail,
                i.interval.start_time,
                i.interval.end_time,
                i.interval.location_id,
                i.interval.is_active

              }),
            t.location_id,
            t.is_active
        }
        )
        .ToArrayAsync();

        var res = data.Select(
            t => new TimeZoneDto(
                t.id,
                t.driver_id,
                t.name,
                t.mode,
                t.active_time,
                t.deactive_time,
                t.intervals.Select(
                    i => new IntervalDto(
                        new DaysInWeekDto(
                            i.days.sun,
                            i.days.mon,
                            i.days.tue,
                            i.days.wed,
                            i.days.thu,
                            i.days.fri,
                            i.days.sat
                            ),
                        i.days_detail,
                        i.start_time,
                        i.end_time,
                        i.location_id,
                        i.is_active
                        )).ToList(),
                t.location_id,
                t.is_active
                )
            ).ToArray();

        return res;
    }

    public async Task<TimeZoneDto> GetByIdAsync(int id)
    {
        var data = await context.timezone
        .AsNoTracking()
        .Where(x => x.id == id)
        .Select(t => new
        {
            t.id,
            t.driver_id,
            t.name,
            t.mode,
            t.active_time,
            t.deactive_time,
            intervals = t.timezone_intervals.Select(i => new
            {
                days = new
                {
                    sun = i.interval.days.sunday,
                    mon = i.interval.days.monday,
                    tue = i.interval.days.tuesday,
                    wed = i.interval.days.wednesday,
                    thu = i.interval.days.thursday,
                    fri = i.interval.days.friday,
                    sat = i.interval.days.saturday
                },
                i.interval.days_detail,
                i.interval.start_time,
                i.interval.end_time,
                i.interval.location_id,
                i.interval.is_active

            }),
            t.location_id,
            t.is_active
            }
        )
    .FirstOrDefaultAsync();

        var res = new TimeZoneDto(
                data.id,
                data.driver_id,
                data.name,
                data.mode,
                data.active_time,
                data.deactive_time,
                data.intervals.Select(
                    i => new IntervalDto(
                        new DaysInWeekDto(
                            i.days.sun,
                            i.days.mon,
                            i.days.tue,
                            i.days.wed,
                            i.days.thu,
                            i.days.fri,
                            i.days.sat
                            ),
                        i.days_detail,
                        i.start_time,
                        i.end_time,
                        i.location_id,
                        i.is_active
                        )).ToList(),
                data.location_id,
                data.is_active
                );

        return res;
    }

    public async Task<IEnumerable<TimeZoneDto>> GetByLocationIdAsync(int locationId)
    {
        var data = await context.timezone
            .AsNoTracking()
            .Where(x => x.location_id == locationId || x.location_id == 1)
            .Select(t => new
            {
                t.id,
                t.driver_id,
                t.name,
                t.mode,
                t.active_time,
                t.deactive_time,
                intervals = t.timezone_intervals.Select(i => new
                {
                    days = new
                    {
                        sun = i.interval.days.sunday,
                        mon = i.interval.days.monday,
                        tue = i.interval.days.tuesday,
                        wed = i.interval.days.wednesday,
                        thu = i.interval.days.thursday,
                        fri = i.interval.days.friday,
                        sat = i.interval.days.saturday
                    },
                    i.interval.days_detail,
                    i.interval.start_time,
                    i.interval.end_time,
                    i.interval.location_id,
                    i.interval.is_active

                }),
                t.location_id,
                t.is_active
            }
                )
            .ToArrayAsync();

        var res = data.Select(
            t => new TimeZoneDto(
                t.id,
                t.driver_id,
                t.name,
                t.mode,
                t.active_time,
                t.deactive_time,
                t.intervals.Select(
                    i => new IntervalDto(
                        new DaysInWeekDto(
                            i.days.sun,
                            i.days.mon,
                            i.days.tue,
                            i.days.wed,
                            i.days.thu,
                            i.days.fri,
                            i.days.sat
                            ),
                        i.days_detail,
                        i.start_time,
                        i.end_time,
                        i.location_id,
                        i.is_active
                        )).ToList(),
                t.location_id,
                t.is_active
                )
            ).ToArray();

        return res;
    }

   

    public async Task<IEnumerable<Mode>> GetCommandAsync()
    {
        var dtos = await context.timezone_command.AsNoTracking().Select(s => new Mode
        {
            Name = s.name,
            Value = s.value,
            Description = s.description,

        }).ToArrayAsync();

        return dtos;
    }

    public async Task<bool> IsAnyById(int id)
    {
        return await context.timezone.AnyAsync(x => x.id == id);
    }

    public async Task<IEnumerable<Mode>> GetModeAsync()
    {
        var dtos = await context.timezone_mode.AsNoTracking().Select(s => new Mode
        {
            Name = s.name,
            Value = s.value,
            Description = s.description,

        }).ToArrayAsync();

        return dtos;
    }

    public async Task<Pagination<TimeZoneDto>> GetPaginationAsync(PaginationParamsWithFilter param, int location)
    {

        var query = context.timezone.AsNoTracking().AsQueryable();


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
                        EF.Functions.ILike(x.active_time, pattern) ||
                        EF.Functions.ILike(x.deactive_time, pattern)

                    );
                }
                else // SQL Server
                {
                    query = query.Where(x =>
                        x.name.Contains(search) ||
                        x.active_time.Contains(search) ||
                        x.deactive_time.Contains(search)
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
           .Select(t => new
           {
               t.id,
               t.driver_id,
               t.name,
               t.mode,
               t.active_time,
               t.deactive_time,
               intervals = t.timezone_intervals.Select(i => new
               {
                   days = new
                   {
                       sun = i.interval.days.sunday,
                       mon = i.interval.days.monday,
                       tue = i.interval.days.tuesday,
                       wed = i.interval.days.wednesday,
                       thu = i.interval.days.thursday,
                       fri = i.interval.days.friday,
                       sat = i.interval.days.saturday
                   },
                   i.interval.days_detail,
                   i.interval.start_time,
                   i.interval.end_time,
                   i.interval.location_id,
                   i.interval.is_active

               }),
               t.location_id,
               t.is_active
           }
                )
            .ToArrayAsync();


        var res = data.Select(
            t => new TimeZoneDto(
                t.id,
                t.driver_id,
                t.name,
                t.mode,
                t.active_time,
                t.deactive_time,
                t.intervals.Select(
                    i => new IntervalDto(
                        new DaysInWeekDto(
                            i.days.sun,
                            i.days.mon,
                            i.days.tue,
                            i.days.wed,
                            i.days.thu,
                            i.days.fri,
                            i.days.sat
                            ),
                        i.days_detail,
                        i.start_time,
                        i.end_time,
                        i.location_id,
                        i.is_active
                        )).ToList(),
                t.location_id,
                t.is_active
                )
            ).ToArray();


        return new Pagination<TimeZoneDto>
        {
            Data = res,
            Page = new PaginationData
            {
                TotalCount = count,
                PageNumber = param.PageNumber,
                PageSize = param.PageSize,
                TotalPage = (int)Math.Ceiling(count / (double)param.PageSize)
            }
        };
    }

    public async Task<short> GetLowestUnassignedNumberAsync(int max)
    {
        var query = context.timezone
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
        return expected;
    }

    public async Task<bool> IsAnyByNameAsync(string name)
    {
        return await context.timezone.AnyAsync(x => x.name.Equals(name));
    }
}
