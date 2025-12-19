using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class WeakPassword
    {
        [Key]
        public int Id { get; set; }
        public string Pattern { get; set; } = string.Empty;
        public int PasswordRuleId { get; set; }
        public PasswordRule PasswordRule { get; set; }
    }
}
