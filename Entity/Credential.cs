using HIDAeroService.Entity.Interface;
using Org.BouncyCastle.Bcpg;
using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class Credential : NoMacBaseEntity,IComponentId
    {
        public short ComponentId { get; set; }
        public short Flag { get; set; }
        public int Bits { get; set; }
        public int IssueCode{ get; set; }
        public int FacilityCode { get; set; }
        public long CardNo{ get; set; }
        public string? Pin{  get; set; }
        public string ActiveDate { get; set; }
        public string DeactiveDate { get; set; }
        public string CardHolderId { get; set; }
        public CardHolder CardHolder { get; set; }
        public ICollection<AccessLevelCredential> AccessLevelCredentials { get; set; }
        public ICollection<HardwareCredential> HardwareCredentials { get; set; }
    }
}
