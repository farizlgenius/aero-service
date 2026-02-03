

using Aero.Application.Interfaces;
using Aero.Domain.Entities;

namespace Aero.Application.DTOs
{
    public sealed class CredentialDto : NoMacBaseEntity
    {
        public int Bits { get; set; }
        public int IssueCode { get; set; }
        public int FacilityCode { get; set; }
        public long CardNo { get; set; }
        public string? Pin { get; set; }
        public string ActiveDate { get; set; } = string.Empty;
        public string? DeactiveDate { get; set; }
        //public CardHolderDto? card_holder { get; set; }
    }
}
