using HIDAeroService.Entity.Interface;
using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class Sensor : BaseEntity
    {
        public Module Module { get; set; }
        public Door SensorDoor { get; set; }
        public short ModuleId { get; set; }
        public short InputNo { get; set; }
        public short InputMode { get; set; }
        public short Debounce { get; set; }
        public short HoldTime { get; set; }
        public short DcHeld { get; set; }

    }
}
