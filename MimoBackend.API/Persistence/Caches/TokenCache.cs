using MimoBackend.API.Models;

namespace MimoBackend.API.Persistence.Caches;

public interface ITokenCache
{
    AuthenticationToken GenerateNewToken(Credentials credentials);
}

public class TokenCache : ITokenCache
{
    private const double ExpirationSeconds = 3600;
    
    protected Dictionary<string, AuthenticationToken> Cache { get; set; } = new();
    
    public AuthenticationToken GenerateNewToken(Credentials credentials)
    {
        var newToken = CreateToken(credentials.username);
        Cache.Add(newToken.Token, newToken);
        return newToken;
    }

    private AuthenticationToken CreateToken(string username)
    {
        return new AuthenticationToken()
        {
            Token = Guid.NewGuid().ToString(),
            Username = username,
            Expires = new DateTimeOffset(DateTime.UtcNow.AddSeconds(ExpirationSeconds)).ToUnixTimeSeconds()
        };
    }

    public bool IsTokenValid(string token)
    {
        var isTokenValid = Cache.ContainsKey(token) && !IsExpired(Cache[token]);
        if (!isTokenValid)
        {
            Cache.Remove(token);
        }
        return isTokenValid;
    }

    private bool IsExpired(AuthenticationToken authenticationToken) 
        => authenticationToken.Expires - new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds() <= 0;

    public string GetUsername(string token) 
        => Cache[token].Username;
}