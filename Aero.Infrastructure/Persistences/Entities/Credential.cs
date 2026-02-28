


using Aero.Domain.Entities;
using Aero.Domain.Interface;
using System.Security;

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
        public string user_id { get; set; } = string.Empty;
        public User user { get; set; }
        public ICollection<DeviceCredential> hardware_credentials { get; set; }

        public Credential(int bits,int issue,int fac,long card,string pin,string active,string deactive,string user_id,int location) : base(location)
        {
            this.bits = bits;
            this.issue_code = issue;
            this.fac_code = fac;
            this.card_no = card;
            this.active_date = active;
            this.deactive_date = deactive;
            this.user_id = user_id;
            this.pin = pin;
            
        }

        public Credential(Aero.Domain.Entities.Credential data) : base(data.LocationId)
        {
            this.bits = data.Bits;
            this.issue_code = data.IssueCode;
            this.fac_code = data.FacilityCode;
            this.card_no = data.CardNo;
            this.active_date = data.ActiveDate;
            this.deactive_date = data.DeactiveDate;
            this.user_id = data.user.UserId;
            this.pin = pin;
            this.updated_date = DateTime.UtcNow;

        }

        public void Update(Aero.Domain.Entities.Credential data)
        {
            this.bits = data.Bits;
            this.issue_code = data.IssueCode;
            this.fac_code = data.FacilityCode;
            this.card_no = data.CardNo;
            this.active_date = data.ActiveDate;
            this.deactive_date = data.DeactiveDate;
            this.user_id = data.user.UserId;
            this.pin = pin;
            this.updated_date = DateTime.UtcNow;

        }
    }
}
