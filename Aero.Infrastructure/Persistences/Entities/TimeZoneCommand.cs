using System.ComponentModel.DataAnnotations;

namespace Aero.Infrastructure.Data.Entities
{
    public class TimeZoneCommand
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public short value { get; set; }
    }
}
