
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

        public CommandAudit(int tagno,int scp_id,string mac,string command,bool is_success,bool is_pending,string nak_reason,int nake_desc_code, int location) : base(location)
        {
            this.tag_no = tagno;
            this.scp_id = scp_id;
            this.mac = mac;
            this.command = command;
            this.is_success = is_success;
            this.is_pending = is_pending;
            this.nak_reason = nak_reason;
            this.nake_desc_code = nake_desc_code;
        }

    }
}
