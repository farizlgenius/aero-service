namespace AeroService.Model
{
    public record RefreshTokenRecord(string HashedToken,string UserId,string Username,DateTime ExpireAt);
    
}
