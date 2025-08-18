namespace HIDAeroService.Entity
{
    public sealed class ar_n_proc
    {
        public int id { get; set; }
        public short scp_id { get; set; }
        public string scp_ip { get; set; }
        public short sio_number { get; set; }
        public short proc_number { get; set; }
        public bool is_available { get; set; }
    }
}
