
using System.ComponentModel.DataAnnotations;
using Aero.Domain.Interface;

namespace Aero.Infrastructure.Persistences.Entities
{
    public sealed class CardHolder : BaseEntity
    {
        [Required]
        public string user_id { get; set; } = string.Empty;
        public string title { get; set; } = string.Empty;
        public string first_name { get; set; } = string.Empty;
        public string middle_name { get; set; } = string.Empty;
        public string last_name { get; set; } = string.Empty;
        public string sex { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string phone { get; set; } = string.Empty;
        public string company { get; set; } = string.Empty;
        public string department { get; set; } = string.Empty;
        public string position { get; set; } = string.Empty;
        public short flag { get; set; }
        public ICollection<CardHolderAdditional> additionals { get; set; }
        public string image_path { get; set; } = string.Empty;
        public ICollection<Credential> credentials { get; set; }
        public ICollection<CardHolderAccessLevel> cardholder_access_levels { get; set; }
    }
}
