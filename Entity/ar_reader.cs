namespace HIDAeroService.Entity
{
    public sealed class ar_reader : ar_base_entity
    {
        public string name { get; set; }
        public string scp_mac { get; set; }
        public short sio_number { get; set; }
        public short reader_number { get; set; }
        public short led_drive_mode { get; set; }
        public short osdp_flag { get; set; }
    }
}
