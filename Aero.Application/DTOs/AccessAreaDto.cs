using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Domain.Interface;


namespace Aero.Application.DTOs
{
    public sealed class AccessAreaDto : NoMacBaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public short component_id { get; set; }
        public short MultiOccupancy { get; set; }
        public short AccessControl { get; set; }
        public short OccControl { get; set; }
        public short OccSet { get; set; }
        public short OccMax { get; set; }
        public short OccUp { get; set; }
        public short OccDown { get; set; }
        public short AreaFlag { get; set; }
    }


}
