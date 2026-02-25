using System;
using Aero.Domain.Entities;

namespace Aero.Infrastructure.Mapper;

public sealed class IntervalMapper
{


      public static Aero.Infrastructure.Data.Entities.Interval ToEf(Interval dto)
      {
            var res =new Aero.Infrastructure.Data.Entities.Interval();
            // Base 
            NoMacBaseMapper.ToEf(dto,res);
            res.component_id = dto.ComponentId;
            res.days_desc = dto.DaysDetail;
            res.start_time = dto.StartTime;
            res.end_time = dto.EndTime;
            res.days = new Infrastructure.Data.Entities.DaysInWeek
                  {
                        sunday = dto.Days.Sunday,
                        monday = dto.Days.Monday,
                        tuesday = dto.Days.Tuesday,
                        wednesday = dto.Days.Wednesday,
                        thursday = dto.Days.Thursday,
                        friday = dto.Days.Friday,
                        saturday = dto.Days.Saturday
                  };

            return res;
      }

      public static void Update(Aero.Infrastructure.Data.Entities.Interval en, Interval dto)
      {
            // Base
            NoMacBaseMapper.Update(dto,en);

            // Detail
            en.days_desc = dto.DaysDetail;
            en.start_time = dto.StartTime;
            en.end_time = dto.EndTime;
            en.days.sunday = dto.Days.Sunday;
            en.days.monday = dto.Days.Monday;
            en.days.tuesday = dto.Days.Tuesday;
            en.days.wednesday = dto.Days.Wednesday;
            en.days.thursday = dto.Days.Thursday;
            en.days.friday = dto.Days.Friday;
            en.days.saturday = dto.Days.Saturday;

      }

}
