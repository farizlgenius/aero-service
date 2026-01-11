using AeroService.DTO.Acr;
using AeroService.Entity.Interface;

namespace AeroService.DTO.CardFormat
{
    public sealed class CardFormatDto : NoMacBaseDto
    {
        public string Uuid { get; set; } = Guid.NewGuid().ToString();
        public required string Name { get; set; }
        public short ComponentId { get; set; }
        public short Facility { get; set; }
        public short Bits { get; set; }
        public short PeLn { get; set; }
        public short PeLoc { get; set; }
        public short PoLn { get; set; }
        public short PoLoc { get; set; }
        public short FcLn { get; set; }
        public short FcLoc { get; set; }
        public short ChLn { get; set; }
        public short ChLoc { get; set; }
        public short IcLn { get; set; }
        public short IcLoc { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
