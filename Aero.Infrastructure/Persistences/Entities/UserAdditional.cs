using System.ComponentModel.DataAnnotations;

namespace Aero.Infrastructure.Persistences.Entities
{
    public sealed class UserAdditional
    {
        [Key]
        public int id { get; set; }
        public User user { get; set; }
        public string user_id { get; set; } = string.Empty;
        public string additional { get; set; } = string.Empty;

        public UserAdditional(string userid,string additional) 
        {
            this.user_id = userid;
            this.additional = additional;
        }
    }
}
