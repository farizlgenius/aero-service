namespace HIDAeroService.DTO.Output
{
    public sealed class ToggleControlPointDto
    {
        public string macAddress { get; set; }
        public short ComponentId { get; set; }
        public short Command { get; set; }
    }
}
