using System.Net;
using Microsoft.AspNetCore.Mvc;
using MimoBackend.API.Models;
using MimoBackend.API.Models.DTOs;
using MimoBackend.API.Persistence.Caches;
using MimoBackend.API.Repositories;

namespace MimoBackend.API.Services;

public interface IAuthorizationService
{
    Task<IActionResult> Authorize(Credentials credentials);
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

    public async Task<IActionResult> Authorize(Credentials credentials)
    {
        var user = await _userRepository.GetUser(credentials.username);
        if (UserNotFound(user))
        {
            return BuildResponse(404);
        }

        if (IsPasswordMatch(user!, credentials))
        {
            var token = GetNewToken(credentials);
            return BuildResponse(200, token);
        }

        return BuildResponse(403);
    }

    private static bool UserNotFound(UserDto? user) 
        => user is null;
    
    private static bool IsPasswordMatch(UserDto user, Credentials credentials) 
        => user.Password == credentials.password;
    
    private AuthenticationToken GetNewToken(Credentials credentials) 
        => _tokenCache.GenerateNewToken(credentials);
}
