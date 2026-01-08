using HIDAeroService.Entity.Interface;
using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class Sensor : BaseEntity
    {
        public Module module { get; set; }
        public Door sensor_door { get; set; }
        public short module_id { get; set; }
        public short input_no { get; set; }
        public short input_mode { get; set; }
        public short debounce { get; set; }
        public short holdtime { get; set; }
        public short dc_held { get; set; }

    }
}
