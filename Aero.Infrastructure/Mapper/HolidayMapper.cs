using System;
using Aero.Infrastructure.Data.Entities;

namespace Aero.Infrastructure.Mapper;

public sealed class HolidayMapper
{
      public static Aero.Infrastructure.Data.Entities.Holiday ToEf(Aero.Domain.Entities.Holiday data)
      {
            var res = new Holiday();
            // Base 
            NoMacBaseMapper.ToEf(data,res);
            res.component_id = data.ComponentId;
            res.year = data.Year;
            res.month = data.Month;
            res.day = data.Day;
            res.extend = data.Extend;
            res.type_mask = data.TypeMask;
            return res;
      }

      public static void Update(Aero.Infrastructure.Data.Entities.Holiday en,Aero.Domain.Entities.Holiday data)
      {
            NoMacBaseMapper.Update(data,en);
            en.year = data.Year;
            en.month = data.Month;
            en.day = data.Day;
            en.extend = data.Extend;
            en.type_mask = data.TypeMask;
            
      }
}
