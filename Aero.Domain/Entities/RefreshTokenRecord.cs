namespace Aero.Domain.Entities
{
    public record RefreshTokenRecord(string HashedToken,string Username,DateTime ExpireAt);
    
}
