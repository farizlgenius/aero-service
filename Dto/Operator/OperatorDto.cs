using HIDAeroService.Entity;

namespace HIDAeroService.DTO.Operator
{
    public sealed class OperatorDto : NoMacBaseDto
    {
        public short ComponentId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;
        public short RoleId { get; set; }
    }
}
