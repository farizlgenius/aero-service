using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class ar_control_point
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public string scp_ip { get; set; }
        public short sio_number { get; set; }
        public short cp_number { get; set; }
        public short op_number {  get; set; } 
        public short mode {  get; set; }
        
    }
}
