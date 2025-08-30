namespace HIDAeroService.Dto.Mp
{
    public sealed class MpDto
    {
        public int No { get; set; }
        public string Name { get; set; }
        public short SioNumber { get; set; }
        public string? SioName { get; set; }
        public string? SioModel { get; set; }
        public short MpNumber { get; set; }
        public short IpNumber { get; set; }
        public string Mode { get; set; }
        public short DelayEntry { get; set; }
        public short DelayExit { get; set; }
        public string ScpMac { get; set; }
        public short Status { get; set; }
    }
}
