
using System.ComponentModel.DataAnnotations;
using Aero.Domain.Interface;

namespace Aero.Infrastructure.Persistences.Entities
{
    public sealed class CommandAudit : BaseEntity
    {

        public int tag_no { get; set; }
        public int scp_id { get; set; }
        public string? mac { get; set; } = string.Empty;
        public string? command { get; set; } = string.Empty;
        public bool is_success { get; set; }
        public bool is_pending { get; set; }
        public string? nak_reason { get; set; }
        public int nake_desc_code { get; set; }

    }
}
