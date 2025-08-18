namespace HIDAeroService.Dto
{
    public class AddMpDto
    {
        public string Name { get; set; }
        public string ScpIp { get; set; }
        public short SioNumber { get; set; }
        public short IpNumber { get; set; }
        public short LcvtMode { get; set; }
        public short Debounce { get; set; }
        public short HoldTime { get; set; }
        public short LfCode { get; set; }
        public short DelayEntry { get; set; }
        public short DelayExit { get; set; }
        public short Mode { get; set; }
    }
}
