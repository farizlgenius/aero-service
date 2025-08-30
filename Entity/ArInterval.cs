using HIDAeroService.Entity.Interface;
using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class ArInterval : ArBaseEntity,IActivatable,IComponentNo
    {
        public short ComponentNo { get; set; }
        public short IDays { get; set; }
        public string Days { get; set; } = string.Empty;
        public short IStart { get; set; }
        public string StartTime { get; set; } = string.Empty;
        public short IEnd { get; set; }
        public string Endtime { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        
    }
}
