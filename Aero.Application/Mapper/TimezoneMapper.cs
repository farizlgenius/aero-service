using System;
using Aero.Application.DTOs;
using Aero.Domain.Entities;

namespace Aero.Application.Mapper;

public class TimezoneMapper
{

      public static Timezone ToDomain(TimeZoneDto dto)
      {
            var res = new Timezone();
            // Base
            NoMacBaseMapper.ToDomain(res,dto);
            res.Name = dto.Name;
            res.Mode = dto.Mode;
            res.ActiveTime = dto.ActiveTime;
            res.DeactiveTime = dto.DeactiveTime;
            res.Intervals = dto.Intervals is null ? new List<Interval>() : dto.Intervals.Select(x => IntervalMapper.ToDomain(x)).ToList();
            return res;
      }
}
