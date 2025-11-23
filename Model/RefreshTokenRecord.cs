namespace HIDAeroService.Model
{
    public record RefreshTokenRecord(string HashedToken,string UserId,DateTime ExpireAt);
    
}
