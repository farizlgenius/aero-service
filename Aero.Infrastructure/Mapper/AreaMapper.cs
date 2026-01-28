using System;
using Aero.Domain.Entities;

namespace Aero.Infrastructure.Mapper;

public sealed class AreaMapper
{
      public static Aero.Infrastructure.Data.Entities.AccessArea ToEf(AccessArea data)
      {
            var res = new Aero.Infrastructure.Data.Entities.AccessArea();
            // Base
            NoMacBaseMapper.ToEf(data,res);
            res.name = data.Name;
            res.multi_occ = data.MultiOccupancy;
            res.access_control = data.AccessControl;
            res.occ_control = data.OccControl;
            res.occ_set = data.OccSet;
            res.occ_max = data.OccMax;
            res.occ_up = data.OccUp;
            res.occ_down = data.OccDown;
            res.area_flag = data.AreaFlag;

            return res;
      }

      public static void Update(AccessArea data,Aero.Infrastructure.Data.Entities.AccessArea res)
      {
            NoMacBaseMapper.Update(data,res);
            res.name = data.Name;
            res.multi_occ = data.MultiOccupancy;
            res.access_control = data.AccessControl;
            res.occ_control = data.OccControl;
            res.occ_set = data.OccSet;
            res.occ_max = data.OccMax;
            res.occ_up = data.OccUp;
            res.occ_down = data.OccDown;
            res.area_flag = data.AreaFlag;
      }

}
