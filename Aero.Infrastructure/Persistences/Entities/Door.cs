
using Aero.Domain.Entities;
using Aero.Domain.Enums;
using Aero.Domain.Interfaces;
using System.Security.AccessControl;
using static System.Net.WebRequestMethods;

namespace Aero.Infrastructure.Persistences.Entities
{
    public sealed class Door : BaseEntity,IDeviceId,IDriverId
    {
        public short driver_id {get; set;}
        public string name { get; set; } = string.Empty;   
        public short access_config { get; set; }
        public short pair_door_no { get; set; }
        public int device_id { get; set; }

        // Door Direction
        public DoorDirection direction { get; set; }
        public Device device { get; set; }
        // Reader setting for Reader In / Reader Out
        public ICollection<Reader> readers { get; set; }
        public short reader_out_config { get; set; }

        // Strike setting for strike
        public Strike strike {  get; set; }

        //sensor setting for sensor
        public Sensor sensor { get; set; }
        
        //sensor setting for rex0 / rex1
        public ICollection<RequestExit>? request_exits { get; set; }
        public short card_format { get; set; } = 255;
        public short antipassback_mode { get; set; }
        public short? antipassback_in { get; set; }
        public short area_in_id {get; set;}
        public AccessArea? area_in { get; set; }
        public short? antipassback_out { get; set; }
        public short area_out_id {get; set;}
        public AccessArea? area_out { get; set; }
        public short spare_tag { get; set; }
        public short access_control_flag { get; set; }
        public short mode { get; set; }
        public string mode_detail { get; set; } = string.Empty;
        public short offline_mode { get; set; }
        public string offline_mode_detail { get; set; } = string.Empty;
        public short default_mode { get; set; }
        public string default_mode_detail { get; set; } = string.Empty;
        public short default_led_mode { get; set; }
        public short pre_alarm { get; set; }
        public short antipassback_delay { get; set; }
        public short strike_t2 { get; set; }
        public short dc_held2 { get; set; }
        public short strike_follow_pulse { get; set; }
        public short strike_follow_delay { get; set; }
        public short n_ext_feature_type { get; set; }
        public short i_lpb_sio { get; set; }
        public short i_lpb_number { get; set; }
        public short i_lpb_long_press { get;set; }
        public short i_lpb_out_sio { get; set; }
        public short i_lpb_out_num { get; set; }
        public short df_filter_time { get; set; }
        public bool is_held_mask { get; set; } = false;
        public bool is_force_mask { get; set; } = false;
        public ICollection<AccessLevelComponent> access_level_component { get; set; }

        public Door(short driver,string name,short accessconfig,DoorDirection direction,short pair_door_no,int device_id,int location,List<Aero.Domain.Entities.Reader> readers,short readeroutconfig,Aero.Domain.Entities.Strike k,Aero.Domain.Entities.Sensor s,List<Aero.Domain.Entities.RequestExit> rexs
            ,short cardformat
            ,short antipassbackmode,
            short antipassbackin,
            short areainid,
            short antipassbackout,
            short areaoutid,short spare,short acsflag,short mode,string modedetail,short offline,string offlinedetail,short defaultmode,string defaultdetail,
            short led,short prealarm,short antipassbackdelay,short striket2,short dcheld2,short strikefollowpulse,short strikefollowdelay,short nextfeature,
            short lpbsio,
            short lpbnum,
            short lpblong,
            short lpboutsio,
            short lpboutnum,
            short filter,
            bool isheldmask,
            bool isforcemask
            ) : base(location)
        {
            this.driver_id = driver;
            this.name = name;
            this.access_config = accessconfig;
            this.pair_door_no = pair_door_no;
            this.device_id = device_id;
            this.direction = direction;
            this.readers = readers.Select(r => new Reader(r.ModuleId,r.DoorId,r.ReaderNo,r.DataFormat,r.KeypadMode,r.LedDriveMode,r.OsdpFlag,r.OsdpBaudrate,r.OsdpDiscover,r.OsdpTracing,r.OsdpAddress,r.OsdpSecureChannel,r.DeviceId,r.LocationId)).ToList();
            this.reader_out_config = readeroutconfig;
            this.strike = new Strike(k.ModuleId,k.DoorId,k.OutputNo,k.RelayMode,k.OfflineMode,k.StrkMax,k.StrkMin,k.StrkMode,k.LocationId);
            this.sensor = new Sensor(s.ModuleId,s.DoorId,s.InputNo,s.InputMode,s.Debounce,s.HoldTime,s.DcHeld,s.LocationId);
            this.request_exits = rexs.Select(x => new RequestExit(x.ModuleId,x.DoorId,x.InputNo,x.InputMode,x.Debounce,x.HoldTime,x.MaskTimeZone,x.LocationId)).ToList();
            this.card_format = cardformat;
            this.antipassback_mode = antipassbackmode;
            this.antipassback_in = antipassbackin;
            this.area_in_id = areainid;
            this.antipassback_out = antipassbackout;
            this.area_out_id = areaoutid;
            this.spare_tag = spare;
            this.access_control_flag = acsflag;
            this.mode = mode;
            this.mode_detail = modedetail;
            this.offline_mode = offline;
            this.offline_mode_detail = offlinedetail;
            this.default_mode = defaultmode;
            this.default_mode_detail = defaultdetail;
            this.default_led_mode = led;
            this.pre_alarm = prealarm;
            this.antipassback_delay = antipassbackdelay;
            this.strike_t2 = striket2;
            this.dc_held2 = dcheld2;
            this.strike_follow_pulse = strikefollowpulse;
            this.strike_follow_delay = strikefollowdelay;
            this.n_ext_feature_type = nextfeature;
            this.i_lpb_sio = lpbsio;
            this.i_lpb_number = lpbnum;
            this.i_lpb_long_press = lpblong;
            this.i_lpb_out_sio = lpboutsio;
            this.i_lpb_out_num = lpboutnum;
            this.df_filter_time = filter;
            this.is_held_mask = isheldmask;
            this.is_force_mask = isforcemask;

        }

        public void Update(Aero.Domain.Entities.Door data)
        {
            this.driver_id = data.DriverId;
            this.name = data.Name;
            this.access_config = data.AccessConfig;
            this.pair_door_no = data.PairDoorNo;
            this.device_id = data.DeviceId;
            this.direction = data.Direction;
            this.readers = data.Readers.Select(r => new Reader(r.ModuleId, r.DoorId, r.ReaderNo, r.DataFormat, r.KeypadMode, r.LedDriveMode, r.OsdpFlag, r.OsdpBaudrate, r.OsdpDiscover, r.OsdpTracing, r.OsdpAddress, r.OsdpSecureChannel, r.DeviceId, r.LocationId)).ToList();
            this.reader_out_config = data.ReaderOutConfiguration;
            this.strike = new Strike(data.Strk.ModuleId, data.Strk.DoorId, data.Strk.OutputNo, data.Strk.RelayMode, data.Strk.OfflineMode, data.Strk.StrkMax, data.Strk.StrkMin, data.Strk.StrkMode, data.Strk.LocationId);
            this.sensor = new Sensor(data.Sensor.ModuleId, data.Sensor.DoorId, data.Sensor.InputNo, data.Sensor.InputMode, data.Sensor.Debounce, data.Sensor.HoldTime, data.Sensor.DcHeld, data.Sensor.LocationId);
            this.request_exits = data.RequestExits.Select(x => new RequestExit(x.ModuleId, x.DoorId, x.InputNo, x.InputMode, x.Debounce, x.HoldTime, x.MaskTimeZone, x.LocationId)).ToList();
            this.card_format = data.CardFormat;
            this.antipassback_mode = data.AntiPassbackMode;
            this.antipassback_in = data.AntiPassBackIn;
            this.area_in_id = data.AreaInId;
            this.antipassback_out = data.AntiPassBackOut;
            this.area_out_id = data.AreaOutId;
            this.spare_tag = data.SpareTags;
            this.access_control_flag = data.AccessControlFlags;
            this.mode = data.Mode;
            this.mode_detail = data.ModeDesc;
            this.offline_mode = data.OfflineMode;
            this.offline_mode_detail = data.OfflineModeDesc;
            this.default_mode = data.DefaultMode;
            this.default_mode_detail = data.DefaultModeDesc;
            this.default_led_mode = data.DefaultLEDMode;
            this.pre_alarm = data.PreAlarm;
            this.antipassback_delay = data.AntiPassbackDelay;
            this.strike_t2 = data.StrkT2;
            this.dc_held2 = data.DcHeld2;
            this.strike_follow_pulse = data.StrkFollowPulse;
            this.strike_follow_delay = data.StrkFollowDelay;
            this.n_ext_feature_type = data.nExtFeatureType;
            this.i_lpb_sio = data.IlPBSio;
            this.i_lpb_number = data.IlPBNumber;
            this.i_lpb_long_press = data.IlPBLongPress;
            this.i_lpb_out_sio = data.IlPBOutSio;
            this.i_lpb_out_num = data.IlPBOutNum;
            this.df_filter_time = data.DfOfFilterTime;
            this.is_held_mask = data.MaskHeldOpen;
            this.is_force_mask = data.MaskForceOpen;
        }


    }
}
