using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class ArCredential : ArBaseEntity
    {
        public string CardHolderRefNo { get; set; }
        public int Bits { get; set; }
        public int IssueCode{ get; set; }
        public int FacilityCode { get; set; }
        public long CardNo{ get; set; }
        public string? Pin{  get; set; }
        public string ActTime{ get; set; }
        public string DeactTime{ get; set; }
        public short AccessLevel { get; set; }
        public string? Image { get; set; }

        public bool IsActive { get; set; }

    }
}
