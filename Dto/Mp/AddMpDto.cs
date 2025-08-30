namespace HIDAeroService.Dto.Mp
{
    public class AddMpDto
    {
        public string Name { get; set; }
        public string ScpMac { get; set; }
        public short SioNo { get; set; }
        public short IpNo { get; set; }
        public short LcvtMode { get; set; }
        public short Debounce { get; set; }
        public short HoldTime { get; set; }
        public short LfCode { get; set; }
        public short DelayEntry { get; set; }
        public short DelayExit { get; set; }
        public short Mode { get; set; }
    }
}
