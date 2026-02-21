using System.ComponentModel.DataAnnotations;

namespace Aero.Infrastructure.Persistences.Entities
{
    public sealed class HardwareComponent 
    {
        [Key]
        public int id { get; set; }
        public short model_no { get; set; }
        public string name { get; set; }
        public short n_input { get; set; }
        public short n_output { get; set; }
        public short n_reader { get; set; }

    }
}
