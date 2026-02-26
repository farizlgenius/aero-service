using System;
using Aero.Infrastructure.Data.Entities;

namespace Aero.Infrastructure.Mapper;

public class TriggerMapper
{
      public static Aero.Infrastructure.Data.Entities.Trigger ToEf(Aero.Domain.Entities.Trigger data)
      {
            var res = new Aero.Infrastructure.Data.Entities.Trigger();
            MacBaseMapper.ToEf(data, res);
            res.driver_id = data.DriverID;
            res.name = data.Name;
            res.procedure_id = data.ProcedureId;
            res.source_type = data.SourceType;
            res.tran_type = data.TranType;
            res.mac = data.Mac;
            res.code_map = data.CodeMap.Select(x => new TriggerTranCode
            {
                  name = x.Name,
                  description = x.Description,
                  value = x.Value,
                  trigger_id = data.ComponentId
            }).ToList();
            res.timezone = data.TimeZone;
            return res;

      }
}
