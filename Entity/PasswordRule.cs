using System.ComponentModel.DataAnnotations;

namespace AeroService.Entity
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
    }
}
