using AeroService.Entity.Interface;
using System.ComponentModel.DataAnnotations;

namespace AeroService.Entity
{
    public sealed class Strike : BaseEntity
    {
        public Module module { get; set; }
        public Door strike_door { get; set; }
        public short module_id { get; set; }
        public short output_no { get; set; }
        public short relay_mode { get; set; }
        public short offline_mode { get; set; }
        public short strike_max { get; set; }
        public short strike_min { get; set; }
        public short strike_mode { get; set; }

    }
}
