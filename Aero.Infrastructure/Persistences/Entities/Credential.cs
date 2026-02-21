


using Aero.Domain.Interface;

namespace Aero.Infrastructure.Persistences.Entities
{
    public sealed class Credential : BaseEntity
    {
        public int bits { get; set; }
        public int issue_code{ get; set; }
        public int fac_code { get; set; }
        public long card_no{ get; set; }
        public string? pin{  get; set; }
        public string active_date { get; set; } = string.Empty;
        public string deactive_date { get; set; } = string.Empty;
        public string cardholder_id { get; set; } = string.Empty;
        public CardHolder cardholder { get; set; }
        public ICollection<HardwareCredential> hardware_credentials { get; set; }
    }
}
