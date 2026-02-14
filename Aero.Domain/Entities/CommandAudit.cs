using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aero.Domain.Entities
{
    public sealed class CommandAudit
    {
        public int TagNo { get; set; }
        public int ScpId { get; set; }
        public string? Mac { get; set; } = string.Empty;
        public string? Command { get; set; } = string.Empty;
        public bool IsPending { get; set; }
        public bool IsSuccess { get; set; }
        public string? NakReason { get; set; }
        public int NakDescCode { get; set; }
        public short LoationId { get; set; }
       

    }
}
