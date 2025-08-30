namespace HIDAeroService.Dto.Cp
{
    public sealed class CpDto
    {
        public int No { get; set; }
        public string Name { get; set; }
        public short SioNumber { get; set; }
        public string? SioName { get; set; }
        public string? SioModel { get; set; }
        public short CpNumber { get; set; }
        public short OpNumber { get; set; }
        public string Mode { get; set; }
        public string ScpMac { get; set; }
        public short Status { get; set; }
    }
}
