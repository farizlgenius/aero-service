using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class ar_acr 
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }   
        public string scp_ip { get; set; }
        public short acr_number { get; set; }
        public short access_cfg { get; set; }
        public short rdr_sio { get; set; }
        public short reader_number { get; set; }
        public short strk_sio { get; set; }
        public short strk_number { get; set; }
        public short strike_t_min { get; set; }
        public short strike_t_max { get; set; }
        public short strike_mode { get; set; }
        public short sensor_sio { get; set; }
        public short sensor_number { get; set; }
        public short dc_held { get; set; }
        public short rex1_sio { get; set; }
        public short rex1_number { get; set; }
        public short rex2_sio { get; set; }
        public short rex2_number { get; set; }
        public short rex1_tzmask { get; set; }
        public short rex2_tzmask { get; set; }
        public short altrdr_sio { get; set; }
        public short altrdr_number { get;set; }
        public short altrdr_spec { get; set; }
        public short cd_format { get; set; }
        public short apb_mode { get; set; }
        public short offline_mode { get; set; }
        public short default_mode { get; set; }
        public short default_led_mode { get; set; }
        public short pre_alarm { get; set; }
        public short apb_delay { get; set; }
        public short strk_t2 { get; set; }
        public short dc_held2 { get; set; }
        public short strk_follow_pulse { get; set; }
        public short strk_follow_delay { get; set; }
        public short nExtFeatureType { get; set; }
        public short ilPB_sio { get; set; }
        public short ilPB_number { get; set; }
        public short ilPB_long_press { get;set; }
        public short ilPB_out_sio { get; set; }
        public short ilPB_out_num { get; set; }
        public short dfofFilterTime { get; set; }

        
    }
}
