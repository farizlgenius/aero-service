using HIDAeroService.Entity.Interface;
using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public class AeroStructureStatus : IDatetime
    {
        [Key]
        public int Id { get; set; }
        public string Uuid { get; set; } = Guid.NewGuid().ToString();
        public string IpAddress { get; set; }
        public int RecAllocTransaction { get; set; }
        public int RecAllocTimezone { get; set; }
        public int RecAllocHoliday { get; set; }
        public int RecAllocSioPort { get; set; }
        public int RecAllocMp { get; set; }
        public int RecAllocCp { get; set; }
        public int RecAllocAcr { get; set; }
        public int RecAllocAlvl { get; set; }
        public int RecAllocTrig { get; set; }
        public int RecAllocProc { get; set; }
        public int RecAllocMpg { get; set; }
        public int RecAllocArea { get; set; }
        public int RecAllocEal { get; set; }
        public int RecAllocCrdb { get; set; }
        public int RecAllocCardActive { get; set; }
        public string MacAddress { get; set; } = string.Empty;
        public short LocationId { get; set; } = 1;
        public Location Location { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
