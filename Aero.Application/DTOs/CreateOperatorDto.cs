namespace Aero.Application.DTOs
{
    public sealed class CreateOperatorDto : NoMacBaseDto
    {
        public short ComponentId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;
        public short RoleId { get; set; }
        public List<short> LocationIds { get; set; } = new List<short>();
    }
}
