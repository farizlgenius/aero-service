using HIDAeroService.Entity.Interface;
using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class Door : BaseEntity
    {
        public string name { get; set; } = string.Empty;   
        public short access_config { get; set; }
        public short pair_door_no { get; set; }
        public string hardware_mac { get; set; } = string.Empty;
        public Hardware hardware { get; set; }
        // Reader setting for Reader In / Reader Out
        public ICollection<Reader> readers { get; set; }
        public short reader_out_config { get; set; }

        // Strike setting for strike
        public short strike_id { get; set; }
        public Strike strike {  get; set; }

        //sensor setting for sensor
        public short sensor_id { get; set; }
        public Sensor sensor { get; set; }
        

        //sensor setting for rex0 / rex1
        public ICollection<RequestExit>? request_exits { get; set; }
        public short card_format { get; set; } = 255;
        public short antipassback_mode { get; set; }
        public short? antipassback_in { get; set; }
        public Area? area_in { get; set; }
        public short? antipassback_out { get; set; }
        public Area? area_out { get; set; }
        public short spare_tag { get; set; }
        public short access_control_flag { get; set; }
        public short mode { get; set; }
        public string mode_desc { get; set; } = string.Empty;
        public short offline_mode { get; set; }
        public string offline_mode_desc { get; set; } = string.Empty;
        public short default_mode { get; set; }
        public string default_mode_desc { get; set; } = string.Empty;
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
        public ICollection<AccessLevelDoorTimeZone> accesslevel_door_timezones { get; set; }

    }
}
