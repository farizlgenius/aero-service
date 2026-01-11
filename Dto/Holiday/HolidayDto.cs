using AeroService.Entity.Interface;

namespace AeroService.DTO.Holiday
{
    public sealed class HolidayDto : NoMacBaseDto,IComponentId
    {
        public short component_id { get; set; }
        public short Year { get; set; }
        public short Month { get; set; }
        public short Day { get; set; }
        public short Extend { get; set; }
        public short TypeMask { get; set; }

    }
}
