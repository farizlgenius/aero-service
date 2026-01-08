using HIDAeroService.DTO.AccessLevel;
using HIDAeroService.DTO.CardHolder;
using HIDAeroService.Entity;
using HIDAeroService.Entity.Interface;

namespace HIDAeroService.DTO.Credential
{
    public sealed class CredentialDto : NoMacBaseDto,IComponentId
    {
        public short component_id { get; set; }
        public int Bits { get; set; }
        public int IssueCode { get; set; }
        public int FacilityCode { get; set; }
        public long CardNo { get; set; }
        public string? Pin { get; set; }
        public string ActiveDate { get; set; }
        public string? DeactiveDate { get; set; }
        //public CardHolderDto? card_holder { get; set; }
    }
}
