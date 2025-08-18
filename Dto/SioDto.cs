namespace HIDAeroService.Dto
{
    public class SioDto
    {
        public int No { get; set; }
        public string Name { get; set; }
        public string ScpName { get; set; }
        public string ScpIp { get; set; }
        public short SioNumber { get; set; }
        public string Model { get; set; }
        public short Address { get; set; }
        public short BaudRate { get; set; }
        public short ProtoCol { get; set; }
        public short Status { get; set; }
        public string Tamper { get; set; }
        public string ACFail { get; set; }
        public string BattFail { get; set; }
    }
}
