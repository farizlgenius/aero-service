using System;
using Aero.Infrastructure.Data.Entities;

namespace Aero.Infrastructure.Mapper;

public sealed class MonitorPointMapper
{
      public static MonitorPoint ToEf(Aero.Domain.Entities.MonitorPoint data)
      {
            var res = new MonitorPoint();
            // Base 
            MacBaseMapper.ToEf(data,res);
            res.mp_id = data.MpId;
            res.name = data.Name;
            res.module_id = data.ModuleId;
            res.input_no = data.InputNo;
            res.input_mode = data.InputMode;
            res.input_mode_desc = data.InputModeDescription;
            res.debounce = data.Debounce;
            res.holdtime = data.HoldTime;
            res.log_function = data.LogFunction;
            res.log_function_desc = data.LogFunctionDescription;
            res.monitor_point_mode = data.MonitorPointMode;
            res.monitor_point_mode_desc = data.MonitorPointModeDescription;
            res.delay_entry = data.DelayEntry;
            res.delay_exit = data.DelayExit;
            res.is_mask = data.IsMask;

            return res;

      }

      public static void Update(Aero.Domain.Entities.MonitorPoint data,Aero.Infrastructure.Data.Entities.MonitorPoint res)
      {
            MacBaseMapper.Update(data,res);
            res.mp_id = data.MpId;
            res.name = data.Name;
            res.module_id = data.ModuleId;
            res.input_no = data.InputNo;
            res.input_mode = data.InputMode;
            res.input_mode_desc = data.InputModeDescription;
            res.debounce = data.Debounce;
            res.holdtime = data.HoldTime;
            res.log_function = data.LogFunction;
            res.log_function_desc = data.LogFunctionDescription;
            res.monitor_point_mode = data.MonitorPointMode;
            res.monitor_point_mode_desc = data.MonitorPointModeDescription;
            res.delay_entry = data.DelayEntry;
            res.delay_exit = data.DelayExit;
            res.is_mask = data.IsMask;
      }

}
