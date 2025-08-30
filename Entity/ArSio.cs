using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class ArSio : ArBaseEntity
    {
        public string Name { get; set; }
        public string ScpName { get; set; }
        public string ScpIp { get; set; }
        public string ScpMac { get; set; }
        public short SioNumber { get; set; }
        public short NInput { get; set; }
        public short NOutput { get; set; }
        public short NReader { get; set; }
        public short Model {  get; set; }
        public string ModeDescription { get; set; }
        public short Address { get; set; }
        public short Msp1No { get; set; }
        public short PortNo { get; set; }
        public short BaudRate { get; set; }
        public short NProtocol { get; set; }
        public short NDialect { get; set; }
    }
}
