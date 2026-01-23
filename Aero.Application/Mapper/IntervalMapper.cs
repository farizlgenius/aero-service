using System;
using Aero.Application.DTOs;

namespace Aero.Application.Mapper;

public sealed class IntervalMapper
{
      public static Aero.Domain.Entities.Interval ToDomain(IntervalDto dto)
      {
            return new Domain.Entities.Interval
            {
                  ComponentId = dto.ComponentId,
                  DaysDesc = dto.DaysDesc,
                  StartTime = dto.StartTime,
                  EndTime = dto.EndTime,
                  Days = new Domain.Entities.DayInWeek
                  {
                        Sunday = dto.Days.Sunday,
                        Monday = dto.Days.Monday,
                        Tuesday = dto.Days.Tuesday,
                        Wednesday = dto.Days.Wednesday,
                        Thursday = dto.Days.Thursday,
                        Friday = dto.Days.Friday,
                        Saturday = dto.Days.Saturday
                  },
                  LocationId = dto.LocationId,
                  HardwareName = dto.HardwareName

            };
      }
}
