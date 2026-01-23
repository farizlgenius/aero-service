using System;
using Aero.Domain.Entities;

namespace Aero.Infrastructure.Mapper;

public sealed class IntervalMapper
{
      

      public static Aero.Infrastructure.Data.Entities.Interval ToEf(Interval dto)
      {
            return new Infrastructure.Data.Entities.Interval
            {
                  component_id = dto.ComponentId,
                  days_desc = dto.DaysDesc,
                  start_time = dto.StartTime,
                  end_time = dto.EndTime,
                  days = new Infrastructure.Data.Entities.DaysInWeek
                  {
                        sunday = dto.Days.Sunday,
                        monday = dto.Days.Monday,
                        tuesday = dto.Days.Tuesday,
                        wednesday = dto.Days.Wednesday,
                        thursday = dto.Days.Thursday,
                        friday = dto.Days.Friday,
                        saturday = dto.Days.Saturday
                  },
                  created_date = DateTime.UtcNow,
                  updated_date = DateTime.UtcNow,
                  location_id = dto.LocationId,
                  is_active = true,

            };
      }

}
