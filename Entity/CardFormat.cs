using HIDAeroService.Entity.Interface;
using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public class CardFormat : IComponentId,IDatetime
    {
        [Key]
        public int Id { get; set; }
        public string Uuid { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = string.Empty;
        public short ComponentId { get; set; }
        public short Facility { get; set; }
        public short Offset { get; set; }
        public short FunctionId { get; set; }
        public short Flags { get; set; }
        public short Bits { get; set; }
        public short PeLn { get; set; }
        public short PeLoc { get; set; }
        public short PoLn { get; set; }
        public short PoLoc { get; set; }
        public short FcLn { get; set; }
        public short FcLoc { get; set; }
        public short ChLn { get; set; }
        public short ChLoc { get; set; }
        public short IcLn { get; set; }
        public short IcLoc { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
