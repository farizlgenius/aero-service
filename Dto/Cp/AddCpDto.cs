namespace HIDAeroService.Dto.Cp
{
    public sealed class AddCpDto
    {
        public string Name { get; set; }
        public short SioNumber { get; set; }
        public short OpNumber { get; set; }
        public short Mode { get; set; }
        public string ScpMac { get; set; }
        public short DefaultPulseTime { get; set; }
    }
}
