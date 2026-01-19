
using AeroService.DTOs;

namespace Aero.Application.DTOs
{
    public sealed class CardHolderDto : NoMacBaseDto
    {
        public required string UserId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Sex { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;
        public short Flag { get; set; }
        public List<string> Additionals { get; set; } = new List<string>();
        public List<CredentialDto>? Credentials { get; set; }
        public List<AccessLevelDto>? AccessLevels { get; set; }

    }
}
