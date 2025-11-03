using HIDAeroService.Entity.Interface;
using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class Strike : BaseEntity
    {
        public Module Module { get; set; }
        public Door StrkDoor { get; set; }
        public short ModuleId { get; set; }
        public short OutputNo { get; set; }
        public short RelayMode { get; set; }
        public short OfflineMode { get; set; }
        public short StrkMax { get; set; }
        public short StrkMin { get; set; }
        public short StrkMode { get; set; }

    }
}
