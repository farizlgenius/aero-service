using System.ComponentModel.DataAnnotations;

namespace Aero.Infrastructure.Persistences.Entities
{
    public sealed class WeakPassword
    {
        [Key]
        public int id { get; set; }
        public string pattern { get; set; } = string.Empty;
        public int password_rule_id { get; set; }
        public PasswordRule password_rule { get; set; }

        public WeakPassword(){}

        public WeakPassword(string pattern,int password_rule_id)
        {
            this.pattern = pattern;
            this.password_rule_id = password_rule_id;
        }
    }
}
