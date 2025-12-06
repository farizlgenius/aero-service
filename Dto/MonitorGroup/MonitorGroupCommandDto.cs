namespace HIDAeroService.DTO.MonitorGroup
{
    public sealed class MonitorGroupCommandDto
    {
        public string MacAddress { get; set; } = string.Empty;
        public short ComponentId { get; set; }
        public short Command { get; set; }
        public short Arg { get; set; }
    }
}
