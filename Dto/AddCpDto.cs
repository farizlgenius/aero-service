namespace HIDAeroService.Dto
{
    public sealed class AddCpDto
    {
        public string Name {  get; set; }
        public short SioNumber { get; set; }
        public short OpNumber { get; set; }
        public short Mode { get; set; }
        public string ScpIp { get; set; }
        public short DefaultPulseTime { get; set; }
    }
}
