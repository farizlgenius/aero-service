namespace Aero.Application.DTOs
{
    public sealed class TokenDto
    {
        public DateTime TimeStamp { get; set; }
        public string AccessToken { get; set; } = string.Empty;
        public int ExpireInMinute {  get; set; }
    }
}
