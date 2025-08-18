namespace HIDAeroService.Dto
{
    public sealed class ScpRegisDto
    {
        public required short ScpId { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public string Mac { get; set; }
        public string Ip { get; set; }
        public string SerialNumber {get; set;}
    }
}
