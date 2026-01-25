using System;
using Aero.Application.DTOs;
using Aero.Domain.Entities;

namespace Aero.Application.Mapper;

public sealed class IntervalMapper
{
      public static Aero.Domain.Entities.Interval ToDomain(IntervalDto dto)
      {
            var res = new Interval();
            // Base
            NoMacBaseMapper.ToDomain(res,dto);
            res.DaysDesc = dto.DaysDesc;
            res.StartTime = dto.StartTime;
            res.EndTime = dto.EndTime;
            res.Days = new Domain.Entities.DaysInWeek
                  {
                        Sunday = dto.Days.Sunday,
                        Monday = dto.Days.Monday,
                        Tuesday = dto.Days.Tuesday,
                        Wednesday = dto.Days.Wednesday,
                        Thursday = dto.Days.Thursday,
                        Friday = dto.Days.Friday,
                        Saturday = dto.Days.Saturday
                  };

            return res;
      }
}
