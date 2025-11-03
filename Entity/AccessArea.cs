using HIDAeroService.Entity.Interface;
using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class AccessArea : BaseEntity
    {
        public string Name { get; set; } = string.Empty;    
        public short MultiOccupancy { get; set; }
        public short AccessControl { get; set; }
        public short OccControl { get; set; }
        public short OccSet { get; set; }
        public short OccMax { get; set; }
        public short OccUp { get; set; }
        public short OccDown { get; set; }
        public short AreaFlag { get; set; }
    }
}
