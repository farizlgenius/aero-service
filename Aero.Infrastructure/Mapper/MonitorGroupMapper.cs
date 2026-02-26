using System;
using Aero.Domain.Entities;

namespace Aero.Infrastructure.Mapper;

public sealed class MonitorGroupMapper
{
      public static Aero.Infrastructure.Data.Entities.MonitorGroup ToEf(MonitorGroup data)
      {
            // base 
            var res = new Aero.Infrastructure.Data.Entities.MonitorGroup();
            MacBaseMapper.ToEf(data, res);
            res.name = data.Name;
            res.n_mp_count = data.nMpCount;
            res.mac = data.Mac;
            res.n_mp_list = data.nMpList
                 .Select(x => new Aero.Infrastructure.Data.Entities.MonitorGroupList
                 {
                       point_type = x.PointType,
                       point_number = x.PointNumber,
                       point_type_desc = x.PointTypeDetail
                 })
                 .ToList();
            return res;

      }

      public static void Update(Aero.Domain.Entities.MonitorGroup from, Aero.Infrastructure.Data.Entities.MonitorGroup to)
      {
            MacBaseMapper.Update(from, to);
            to.name = from.Name;
            to.n_mp_count = from.nMpCount;
            to.n_mp_list = from.nMpList
                  .Select(x => new Aero.Infrastructure.Data.Entities.MonitorGroupList
                  {
                        point_type = x.PointType,
                        point_number = x.PointNumber,
                        point_type_desc = x.PointTypeDetail
                  })
                  .ToList();
      }

}
