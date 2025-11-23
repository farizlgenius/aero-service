namespace HIDAeroService.DTO.Token
{
    public sealed class TokenDto
    {
        public DateTime TimeStamp { get; set; }
        public string AccessToken { get; set; }
        //public string RefreshToken { get; set; }
        //public int Expire {  get; set; }
    }
}
