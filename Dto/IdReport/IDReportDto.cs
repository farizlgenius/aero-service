namespace HIDAeroService.DTO.IdReport
{
    public sealed class IDReportDto
    {
        public short DeviceId { get; set; }
        public int SerialNumber { get; set; }
        public short ScpId { get; set; }
        public byte ConfigFlag { get; set; }
        public string MacAddress { get; set; }
        public string Ip { get; set; }
        public short Port { get; set; }
        public string Model { get; set; }
    }
}
