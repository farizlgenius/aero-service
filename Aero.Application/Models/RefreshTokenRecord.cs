namespace Aero.Application.Models
{
    public record RefreshTokenRecord(string HashedToken,string UserId,string Username,DateTime ExpireAt);
    
}
