namespace Aero.Domain.Entities
{
    public record RefreshTokenRecord(string HashedToken,string UserId,string Username,DateTime ExpireAt);
    
}
