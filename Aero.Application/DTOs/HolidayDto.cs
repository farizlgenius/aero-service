using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Domain.Interface;

namespace Aero.Application.DTOs
{
    public sealed class HolidayDto : NoMacBaseEntity
    {
        public short Year { get; set; }
        public short Month { get; set; }
        public short Day { get; set; }
        public short Extend { get; set; }
        public short TypeMask { get; set; }

    }
}
