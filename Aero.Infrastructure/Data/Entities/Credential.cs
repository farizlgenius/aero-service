


using Aero.Infrastructure.Data.Interface;

namespace Aero.Infrastructure.Data.Entities
{
    public sealed class Credential : NoMacBaseEntity,IComponentId
    {
        public short component_id { get; set; }
        public int bits { get; set; }
        public int issue_code{ get; set; }
        public int fac_code { get; set; }
        public long card_no{ get; set; }
        public string? pin{  get; set; }
        public string active_date { get; set; }
        public string deactive_date { get; set; }
        public string cardholder_id { get; set; }
        public CardHolder cardholder { get; set; }
        public ICollection<HardwareCredential> hardware_credentials { get; set; }
    }
}
