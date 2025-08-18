using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class ar_sio 
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public string scp_name { get; set; }
        public string scp_ip { get; set; }
        public short sio_number { get; set; }
        public short n_inputs { get; set; }
        public short n_outputs { get; set; }
        public short n_readers { get; set; }
        public short model {  get; set; }
        public string model_desc { get; set; }
        public short address { get; set; }
        public short msp1_number { get; set; }
        public short port_number { get; set; }
        public short baud_rate { get; set; }
        public short n_protocol { get; set; }
        public short n_dialect { get; set; }
    }
}
