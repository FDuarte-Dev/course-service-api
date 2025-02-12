using MimoBackend.API.Models;

namespace MimoBackend.API.Persistence.Caches;

public interface ITokenCache
{
    AuthenticationToken GenerateNewToken(Credentials credentials);
}

public class TokenCache : ITokenCache
{
    public AuthenticationToken GenerateNewToken(Credentials credentials)
    {
        throw new NotImplementedException();
    }
}