using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class ArControlPoint : ArBaseEntity
    {
        public string Name { get; set; }
        public string ScpMac { get; set; }
        public short SioNo { get; set; }
        public short CpNo { get; set; }
        public short OpNo {  get; set; } 
        public short Mode {  get; set; }
        public short DefaultPulseTime { get; set; }


    }
}
