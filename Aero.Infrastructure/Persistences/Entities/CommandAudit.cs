
using System.ComponentModel.DataAnnotations;
using Aero.Domain.Interface;

namespace Aero.Infrastructure.Persistences.Entities
{
    public sealed class CommandAudit : BaseEntity
    {
        public int tag_no { get; set; }
        public int device_id { get; set; }
        public string? mac { get; set; } = string.Empty;
        public string? command { get; set; } = string.Empty;
        public bool is_success { get; set; }
        public bool is_pending { get; set; }
        public string? nak_reason { get; set; }
        public int nake_desc_code { get; set; }

        public CommandAudit(){}


        public CommandAudit(int tag_no,int device_id,string mac,string command,bool is_success,bool is_pending,string nak_reason,int nake_desc_code, int location_id) : base(location_id)
        {
            this.tag_no = tag_no;
            this.device_id = device_id;
            this.mac = mac;
            this.command = command;
            this.is_success = is_success;
            this.is_pending = is_pending;
            this.nak_reason = nak_reason;
            this.nake_desc_code = nake_desc_code;
        }

        public CommandAudit(Aero.Domain.Entities.CommandAudit data) : base(data.LoationId)
        {
            this.tag_no = data.TagNo;
            this.device_id = data.ScpId;
            this.mac = data.Mac;
            this.command = data.Command;
            this.is_success = data.IsSuccess;
            this.is_pending = data.IsPending;
            this.nak_reason = data.NakReason;
            this.nake_desc_code = data.NakDescCode;
        }

        public void Update(Aero.Domain.Entities.CommandAudit data)
        {
            this.tag_no = data.TagNo;
            this.device_id = data.ScpId;
            this.mac = data.Mac;
            this.command = data.Command;
            this.is_success = data.IsSuccess;
            this.is_pending = data.IsPending;
            this.nak_reason = data.NakReason;
            this.nake_desc_code = data.NakDescCode;
            this.updated_date = DateTime.UtcNow;
        }

    }
}

