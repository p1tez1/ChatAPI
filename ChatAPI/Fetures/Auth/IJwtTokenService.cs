namespace ChatAPI.Fetures.Auth
{
    public interface IJwtTokenService
    {
        string GenerateToken(string username);
    }
}
