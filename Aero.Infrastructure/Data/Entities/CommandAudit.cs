
using System.ComponentModel.DataAnnotations;
using Aero.Domain.Interface;

namespace Aero.Infrastructure.Data.Entities
{
    public sealed class CommandAudit : IDatetime
    {
        [Key]
        public int id { get; set; }
        public int tag_no { get; set; }
        public int scp_id { get; set; }
        public string? mac { get; set; } = string.Empty;
        public string? command { get; set; } = string.Empty;
        public bool is_success { get; set; }
        public bool is_pending { get; set; }
        public string? nak_reason { get; set; }
        public int nake_desc_code { get; set; }
        public short location_id { get; set; }
        public Location location { get; set; }
        public DateTime created_date { get; set; }
        public DateTime updated_date { get; set; }
    }
}
