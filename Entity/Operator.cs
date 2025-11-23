using HIDAeroService.Entity.Interface;
using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class Operator : NoMacBaseEntity,IComponentId
    {
        public short ComponentId { get; set; }
        public required string UserId { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; } 
        public string Email { get; set; } 
        public string Title { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;
        public short RoleId { get; set; }
        public Role Role { get; set; }

    }
}
