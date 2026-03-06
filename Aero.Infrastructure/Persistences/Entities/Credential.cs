


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

        public Credential(){}


        public Credential(int bits,int issue_code,int fac_code,long card_no,string pin,string active_date,string deactive_date,string user_id,int location_id) : base(location_id)
        {
            this.bits = bits;
            this.issue_code = issue_code;
            this.fac_code = fac_code;
            this.card_no = card_no;
            this.active_date = active_date;
            this.deactive_date = deactive_date;
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
            this.user_id = data.UserId;
            this.pin = data.Pin;
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
            this.user_id = data.UserId;
            this.pin = data.Pin;
            this.updated_date = DateTime.UtcNow;

        }
    }
}

