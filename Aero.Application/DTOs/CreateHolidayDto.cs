using AeroService.DTOs;

namespace Aero.Application.DTOs
{
    public class CreateHolidayDto : NoMacBaseDto
    {
        public short Year { get; set; }
        public short Month { get; set; }
        public short Day { get; set; }
        public short Extend { get; set; }
        public short TypeMask { get; set; }

    }
}
