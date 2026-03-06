using Aero.Domain.Helpers;
using System;

namespace Aero.Domain.Entities;

public sealed class Credential : BaseDomain
{
       public int Bits { get; set; }
        public int IssueCode { get; set; }
        public int FacilityCode { get; set; }
        public long CardNo { get; set; }
        public string Pin { get; set; } = string.Empty;
        public string ActiveDate { get; set; } = string.Empty;
        public string DeactiveDate { get; set; } = string.Empty;
        public string UserId {get; set;} = string.Empty;

        public Credential() { }

        public Credential(int bits, int issueCode, int facilityCode, long cardNo, string pin, string activeDate, string deactiveDate,string UserId)
        {
                SetBits(bits);
                SetIssueCode(issueCode);
                SetFacCode(facilityCode);
                SetCardNo(cardNo);
                Pin = ValidateRequiredString(pin, nameof(pin));
                ActiveDate = ValidateRequiredString(activeDate, nameof(activeDate));
                DeactiveDate = ValidateRequiredString(deactiveDate, nameof(deactiveDate));
                SetUserId(UserId);
       
        }

        private void SetUserId(string user)
        {
                ArgumentException.ThrowIfNullOrWhiteSpace(user);
                if(!RegexHelper.IsValidOnlyCharAndDigit(user)) throw new ArgumentException("UserId invalid.",nameof(user));
                this.UserId = user;                
        }

        private void SetBits(int bit)
        {
                if(bit < 0) throw new ArgumentException("Bits invalid.",nameof(bit));
                this.Bits = bit;
        }

        private void SetIssueCode(int issue)
        {
                if(issue < 0) throw new ArgumentException("Issue code invalid.",nameof(issue));
                this.IssueCode = issue;
        }

        private void SetFacCode(int fac)
        {
                if(fac < 0) throw new ArgumentException("Fac code invalid.",nameof(fac));
                this.FacilityCode = fac;
        }

        private void SetCardNo(long card)
        {
                if(card <= 0) throw new ArgumentException("Card no invalid.",nameof(card));
                this.CardNo = card;
        }

        private static string ValidateRequiredString(string value, string field)
        {
                ArgumentException.ThrowIfNullOrWhiteSpace(value, field);
                var trimmed = value.Trim();
                var sanitized = trimmed.Replace("-", string.Empty).Replace("_", string.Empty).Replace(".", string.Empty).Replace(":", string.Empty).Replace("/", string.Empty);
                if (!RegexHelper.IsValidName(trimmed) && !RegexHelper.IsValidOnlyCharAndDigit(sanitized))
                {
                        throw new ArgumentException($"{field} invalid.", field);
                }

                return value;
        }
}
