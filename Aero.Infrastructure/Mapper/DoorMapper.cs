using System;
using Aero.Application.DTOs;
using Aero.Domain.Entities;
using Pipelines.Sockets.Unofficial.Arenas;

namespace Aero.Infrastructure.Mapper;

public sealed class DoorMapper
{
      public static Aero.Infrastructure.Data.Entities.Door ToEf(Door data)
      {
            var en = new Aero.Infrastructure.Data.Entities.Door();
            MacBaseMapper.ToEf(data,en);
            en.acr_id = data.AcrId;
            en.name = data.Name;
            en.access_config =data.AccessConfig;
            en.pair_door_no = data.PairDoorNo;
            en.hardware_mac = data.Mac;
            en.readers = data.Readers.Count == 0 ? new List<Aero.Infrastructure.Data.Entities.Reader>() : data.Readers.Select(x => 
             new Aero.Infrastructure.Data.Entities.Reader
            {
                  module_id = x.ModuleId,
                  reader_no = x.ReaderNo,
                  data_format = x.DataFormat,
                  keypad_mode = x.KeypadMode,
                  led_drive_mode = x.LedDriveMode,
                  direction = x.Direction,
                  osdp_flag = x.OsdpFlag,
                  osdp_baudrate = x.OsdpBaudrate,
                  osdp_discover = x.OsdpDiscover,
                  osdp_tracing = x.OsdpTracing,
                  osdp_address = x.OsdpAddress,
                  osdp_secure_channel = x.OsdpSecureChannel
            }
            ).ToArray();
            en.reader_out_config = data.ReaderOutConfiguration;
            en.strike_id = data.StrkComponentId;
            en.strike = new Aero.Infrastructure.Data.Entities.Strike
            {
                  module_id = data.Strk.ModuleId,
                  output_no = data.Strk.OutputNo,
                  relay_mode = data.Strk.RelayMode,
                  offline_mode = data.Strk.OfflineMode,
                  strike_max = data.Strk.StrkMax,
                  strike_min = data.Strk.StrkMin,
                  strike_mode = data.Strk.StrkMode
            };
            en.sensor_id = data.SensorComponentId;
            en.sensor =  new Aero.Infrastructure.Data.Entities.Sensor
            {
                module_id = data.Sensor.ModuleId,
                input_no = data.Sensor.InputNo,
                input_mode = data.Sensor.InputMode,
                debounce = data.Sensor.Debounce,
                holdtime = data.Sensor.HoldTime,
                dc_held = data.Sensor.DcHeld  
            };
            en.request_exits = data.RequestExits is null || data.RequestExits.Count == 0 ? new List<Aero.Infrastructure.Data.Entities.RequestExit>() : data.RequestExits.Select(x => 
                  new Aero.Infrastructure.Data.Entities.RequestExit
                  {
                        module_id = x.ModuleId,
                        input_no = x.InputNo,
                        input_mode = x.InputMode,
                        debounce = x.Debounce,
                        holdtime = x.HoldTime,
                        mask_timezone = x.MaskTimeZone
                  }
            ).ToList();
            en.card_format = data.CardFormat;
            en.antipassback_mode = data.AntiPassbackMode;
            en.antipassback_in = data.AntiPassBackIn;
            en.area_in_id = data.AreaInId;
            en.area_out_id = data.AreaOutId;
            en.spare_tag = data.SpareTags;
            en.access_control_flag = data.AccessControlFlags;
            en.mode = data.Mode;
            en.mode_desc = data.ModeDesc;
            en.offline_mode = data.OfflineMode;
            en.offline_mode_desc = data.OfflineModeDesc;
            en.default_mode = data.DefaultMode;
            en.default_mode_desc = data.DefaultModeDesc;
            en.default_led_mode = data.DefaultLEDMode;
            en.pre_alarm = data.PreAlarm;
            en.antipassback_delay = data.AntiPassbackDelay;
            en.strike_t2 = data.StrkT2;
            en.dc_held2 = data.DcHeld2;
            en.strike_follow_pulse = data.StrkFollowPulse;
            en.strike_follow_delay = data.StrkFollowDelay;
            en.n_ext_feature_type = data.nExtFeatureType;
            en.i_lpb_sio = data.IlPBSio;
            en.i_lpb_number = data.IlPBNumber;
            en.i_lpb_long_press = data.IlPBLongPress;
            en.i_lpb_out_sio = data.IlPBOutSio;
            en.i_lpb_out_num = data.IlPBOutNum;
            en.df_filter_time = data.DfOfFilterTime;
            en.is_held_mask = data.MaskHeldOpen;
            en.is_force_mask = data.MaskForceOpen;

            return en;
            

      }
}
