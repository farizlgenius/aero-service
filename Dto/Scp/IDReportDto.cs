namespace HIDAeroService.Dto.Scp
{
    public sealed class IDReportDto
    {
        public short DeviceID { get; set; }
        public int SerialNumber { get; set; }
        public short ScpID { get; set; }
        public byte ConfigFlag { get; set; }
        public string MacAddress { get; set; }
        public string Ip { get; set; }
        public short Port { get; set; }
        public string Model { get; set; }
        public bool IsReset { get; set; }
        public bool IsUpload { get; set; }
    }
}
