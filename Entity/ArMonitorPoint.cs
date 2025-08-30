using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class ArMonitorPoint : ArBaseEntity
    {
        public string Name { get; set; }
        public string ScpIp { get; set; }
        public string ScpMac { get; set; }
        public short SioNo { get; set; }
        public short MpNo { get; set; }
        public short IpNo { get; set; }
        public short IcvtNo { get; set; }
        public short LfCode { get; set; }
        public short DelayEntry { get; set; }
        public short DelayExit { get; set; }
    }
}
