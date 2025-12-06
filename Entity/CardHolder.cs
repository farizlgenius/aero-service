

using HIDAeroService.Entity.Interface;
using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class CardHolder : NoMacBaseEntity
    {
        [Required]
        public string UserId { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Sex { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty; 
        public string Company {  get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public short Flag { get; set; }
        public ICollection<CardHolderAdditional> Additional { get; set; }
        public string ImagePath { get; set; } = string.Empty;
        public ICollection<Credential> Credentials { get; set; }
        public ICollection<CardHolderAccessLevel> CardHolderAccessLevels { get; set; }
    }
}
