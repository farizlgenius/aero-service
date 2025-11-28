using HIDAeroService.DTO.Location;

namespace HIDAeroService.DTO.Operator
{
    public sealed class OperatorDto 
    {
        public string Uuid { get; set; } = Guid.NewGuid().ToString();
        public short ComponentId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public short RoleId { get; set; }
        public List<short> LocationIds { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
