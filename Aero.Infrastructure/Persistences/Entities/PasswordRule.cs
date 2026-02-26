using System.ComponentModel.DataAnnotations;

namespace Aero.Infrastructure.Persistences.Entities
{
    public sealed class PasswordRule
    {
        [Key]
        public int id { get; set; }
        public int len { get; set; }
        public bool is_lower { get; set; }
        public bool is_upper { get; set; }
        public bool is_digit { get; set; }
        public bool is_symbol { get; set; }
        public ICollection<WeakPassword> weaks { get; set; }

        public PasswordRule(){}

        public PasswordRule(Aero.Domain.Entities.PasswordRule data)
        {
            this.len = data.Len;
            this.is_lower = data.IsLower;
            this.is_upper = data.IsUpper;
            this.is_digit = data.IsDigit;
            this.is_symbol = data.IsSymbol;
            this.weaks = data.Weaks.Select(w => new WeakPassword(w,data.Id)).ToList();
        }

        public void Update(Aero.Domain.Entities.PasswordRule data)
        {
            this.len = data.Len;
            this.is_lower = data.IsLower;
            this.is_upper = data.IsUpper;
            this.is_digit = data.IsDigit;
            this.is_symbol = data.IsSymbol;
            this.weaks = data.Weaks.Select(w => new WeakPassword(w,data.Id)).ToList();
        }
    }
}
