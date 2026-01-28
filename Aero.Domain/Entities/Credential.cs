using System;

namespace Aero.Domain.Entities;

public sealed class Credential : NoMacBaseEntity
{
       public int Bits { get; set; }
        public int IssueCode { get; set; }
        public int FacilityCode { get; set; }
        public long CardNo { get; set; }
        public string Pin { get; set; } = string.Empty;
        public string ActiveDate { get; set; } = string.Empty;
        public string DeactiveDate { get; set; } = string.Empty;
        //public CardHolderDto? card_holder { get; set; }
}
