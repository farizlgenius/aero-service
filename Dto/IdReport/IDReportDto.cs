namespace AeroService.DTO.IdReport
{
    public sealed class IdReportDto
    {
        public short ComponentId { get; set; }
        public int SerialNumber { get; set; }
        public string MacAddress { get; set; } = string.Empty;
        public string Ip { get; set; } = string.Empty;
        public string Port { get; set; } = string.Empty;
        public string Firmware { get; set; } = string.Empty;
        public short HardwareType { get; set; }
        public string HardwareTypeDescription { get; set; } = string.Empty;
    }
}
