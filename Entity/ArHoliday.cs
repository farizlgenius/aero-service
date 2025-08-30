using HIDAeroService.Entity.Interface;

namespace HIDAeroService.Entity
{
    public sealed class ArHoliday : ArBaseEntity, IComponentNo, IActivatable
    {
        public short ComponentNo { get; set; }
        public short Year { get; set; }
        public short Month { get; set; }
        public short Day { get; set; }
        public short Extend { get; set; }
        public short TypeMask { get; set; }
        public bool IsActive { get; set; }
    }
}
