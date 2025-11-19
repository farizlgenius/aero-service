using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class Operator : BaseEntity
    {
        public string UserId  { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Image { get; set; }
        public short RoleId { get; set; }
        public Role Role { get; set; }

    }
}
