using AeroService.Entity.Interface;
using System.ComponentModel.DataAnnotations;

namespace AeroService.Entity
{
    public sealed class CommandLog : IDatetime
    {
        [Key]
        public int id { get; set; }
        public string uuid { get; set; } = Guid.NewGuid().ToString();
        public int tag_no { get; set; }
        public int hardware_id { get; set; }
        public string? hardware_mac { get; set; } = string.Empty;
        public string? command { get; set; } = string.Empty;
        public char status { get; set; }   
        public string? nak_reason { get; set; }
        public int nake_desc_code { get; set; }
        public bool is_active { get; set; } = true;
        public DateTime created_date { get; set; }
        public DateTime updated_date { get; set; }
    }
}
