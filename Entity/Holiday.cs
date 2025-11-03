using HIDAeroService.Entity.Interface;

namespace HIDAeroService.Entity
{
    public sealed class Holiday : NoMacBaseEntity,IComponentId
    {
        public short ComponentId { get; set; }
        public short Year { get; set; }
        public short Month { get; set; }
        public short Day { get; set; }
        public short Extend { get; set; }
        public short TypeMask { get; set; }
    }
}
