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
        public User user { get; set; } = new User();

        public Credential() { }

        public Credential(int bits, int issueCode, int facilityCode, long cardNo, string pin, string activeDate, string deactiveDate, User user)
        {
                Bits = bits;
                IssueCode = issueCode;
                FacilityCode = facilityCode;
                CardNo = cardNo;
                Pin = ValidateRequiredString(pin, nameof(pin));
                ActiveDate = ValidateRequiredString(activeDate, nameof(activeDate));
                DeactiveDate = ValidateRequiredString(deactiveDate, nameof(deactiveDate));
                this.user = user ?? throw new ArgumentNullException(nameof(user));
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
