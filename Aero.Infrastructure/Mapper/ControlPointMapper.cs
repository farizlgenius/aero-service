using System;
using Aero.Domain.Entities;

namespace Aero.Infrastructure.Mapper;

public sealed class ControlPointMapper
{
      public static Aero.Infrastructure.Data.Entities.ControlPoint ToEf(ControlPoint data)
      {
            var res = new Aero.Infrastructure.Data.Entities.ControlPoint();
            // Base 
            MacBaseMapper.ToEf(data,res);
            res.name = data.Name;
            res.cp_id = data.CpId;
            res.module_id = data.ModuleId;
            res.module_desc = data.ModuleDescription;
            res.output_no = data.OutputNo;
            res.relay_mode = data.RelayMode;
            res.relay_mode_desc  =data.RelayModeDescription;
            res.offline_mode = data.OfflineMode;
            res.offline_mode_desc = data.OfflineModeDescription;
            res.default_pulse = data.DefaultPulse;

            return res;
      }

      public static void Update(Aero.Infrastructure.Data.Entities.ControlPoint res, ControlPoint data){
            MacBaseMapper.Update(data,res);
            res.name = data.Name;
            res.cp_id = data.CpId;
            res.module_id = data.ModuleId;
            res.module_desc = data.ModuleDescription;
            res.output_no = data.OutputNo;
            res.relay_mode = data.RelayMode;
            res.relay_mode_desc = data.RelayModeDescription;
            res.offline_mode = data.OfflineMode;
            res.offline_mode_desc = data.OfflineModeDescription;
            res.default_pulse = data.DefaultPulse;
      }

}
