namespace HIDAeroService.Dto.CardFormat
{
    public sealed class CardFormatDto
    {
        public required string CardFormatName { get; set; }
        public short CardFormatNo { get; set; }
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
    }
}
