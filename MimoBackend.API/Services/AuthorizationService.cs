using Microsoft.AspNetCore.Mvc;
using MimoBackend.API.Models;
using MimoBackend.API.Models.DatabaseObjects;
using MimoBackend.API.Persistence.Caches;
using MimoBackend.API.Repositories;

namespace MimoBackend.API.Services;

public interface IAuthorizationService
{
    IActionResult Authorize(Credentials credentials);
}

public class AuthorizationService : BaseService, IAuthorizationService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenCache _tokenCache;

    public AuthorizationService(IUserRepository userRepository, ITokenCache tokenCache)
    {
        _userRepository = userRepository;
        _tokenCache = tokenCache;
    }

    public IActionResult Authorize(Credentials credentials)
    {
        var user = _userRepository.GetUserBy(credentials.username);
        if (UserNotFound(user))
        {
            return BuildResponse(StatusCodes.Status404NotFound);
        }

        if (IsPasswordMatch(user!, credentials))
        {
            var token = GetNewToken(credentials);
            return BuildResponse(StatusCodes.Status200OK, token);
        }

        return BuildResponse(StatusCodes.Status403Forbidden);
    }

    private static bool UserNotFound(User? user) 
        => user is null;
    
    private static bool IsPasswordMatch(User user, Credentials credentials) 
        => user.Password == credentials.password;
    
    private AuthenticationToken GetNewToken(Credentials credentials) 
        => _tokenCache.GenerateNewToken(credentials);
}
