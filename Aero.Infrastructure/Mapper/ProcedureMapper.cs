using System;
using Aero.Infrastructure.Data.Entities;

namespace Aero.Infrastructure.Mapper;

public sealed class ProcedureMapper
{
      public static Aero.Infrastructure.Data.Entities.Procedure ToEf(Aero.Domain.Entities.Procedure data)
      {
            return new Aero.Infrastructure.Data.Entities.Procedure
            {
                  proc_id = data.DriverId,
                  component_id = data.ComponentId,
                  name = data.Name,
                  location_id = data.LocationId,
                  actions = data.Actions.Select(x => new Aero.Infrastructure.Data.Entities.Action
                  {
                        // Base 
                        component_id = x.ComponentId,
                        mac = x.Mac,
                        location_id = x.LocationId,
                        is_active = true,
                        created_date = DateTime.UtcNow,
                        updated_date = DateTime.UtcNow,

                        scp_id = x.DeviceId,
                        action_type = x.ActionType,
                        action_type_desc = x.ActionTypeDetail,
                        arg1 = x.Arg1,
                        arg2 = x.Arg2,
                        arg3 = x.Arg3,
                        arg4 = x.Arg4,
                        arg5 = x.Arg5,
                        arg6 = x.Arg6,
                        arg7 = x.Arg7,
                        str_arg = x.StrArg,
                        delay_time = x.DelayTime,
                        procedure_id = data.ComponentId
                  }).ToList(),
                  created_date = DateTime.UtcNow,
                  updated_date = DateTime.UtcNow
            };
      }

      public static void Update(Aero.Domain.Entities.Procedure from,Aero.Infrastructure.Data.Entities.Procedure to)
      {
            to.name = from.Name;
            to.location_id = from.LocationId;
            to.actions = from.Actions.Select(x => new Aero.Infrastructure.Data.Entities.Action
                  {
                        // Base 
                        component_id = x.ComponentId,
                        mac = x.Mac,
                        location_id = x.LocationId,
                        is_active = true,
                        created_date = DateTime.UtcNow,
                        updated_date = DateTime.UtcNow,

                        scp_id = x.DeviceId,
                        action_type = x.ActionType,
                        action_type_desc = x.ActionTypeDetail,
                        arg1 = x.Arg1,
                        arg2 = x.Arg2,
                        arg3 = x.Arg3,
                        arg4 = x.Arg4,
                        arg5 = x.Arg5,
                        arg6 = x.Arg6,
                        arg7 = x.Arg7,
                        str_arg = x.StrArg,
                        delay_time = x.DelayTime,
                        procedure_id = from.ComponentId
                  }).ToList();

            to.updated_date = DateTime.UtcNow;
      }

}
