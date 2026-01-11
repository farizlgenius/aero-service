namespace AeroService.DTO.Operator
{
    public sealed class PasswordDto
    {
        public string Username { get; set; } = string.Empty;
        public string Old { get; set; } = string.Empty;
        public string New { get; set; } = string.Empty;
        public string Con { get; set; } = string.Empty;
    }
}
