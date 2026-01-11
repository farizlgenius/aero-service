

using AeroService.Entity.Interface;
using System.ComponentModel.DataAnnotations;

namespace AeroService.Entity
{
    public sealed class CardHolder : NoMacBaseEntity
    {
        [Required]
        public string user_id { get; set; }
        public string title { get; set; } = string.Empty;
        public string first_name { get; set; } = string.Empty;
        public string middle_name { get; set; } = string.Empty;
        public string last_name { get; set; } = string.Empty;
        public string sex { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string phone { get; set; } = string.Empty; 
        public string company {  get; set; } = string.Empty;
        public string department { get; set; } = string.Empty;
        public string position { get; set; } = string.Empty;
        public short flag { get; set; }
        public ICollection<CardHolderAdditional> additional { get; set; }
        public string image_path { get; set; } = string.Empty;
        public ICollection<Credential> credentials { get; set; }
        public ICollection<CardHolderAccessLevel> access_levels { get; set; }
    }
}
