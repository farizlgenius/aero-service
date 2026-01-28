using System;

namespace Aero.Domain.Entities;

public sealed class CardHolder : NoMacBaseEntity
{
       public string UserId { get; set; } = string.Empty;
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
        public List<Credential> Credentials { get; set; } = new List<Credential>();
        public List<AccessLevel> AccessLevels { get; set; } = new List<AccessLevel>();

}
