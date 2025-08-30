namespace HIDAeroService.Dto.Scp
{
    public sealed class ScpDto
    {
        public int No { get; set; }
        public short ScpId { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public string Mac { get; set; }
        public short Port { get; set; }
        public string IpAddress { get; set; }
        public string SerialNumber { get; set; }
        public short Status { get; set; }
        public bool IsReset { get; set; }
        public bool IsUpload { get; set; }
    }
}
