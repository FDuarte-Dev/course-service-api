using Microsoft.AspNetCore.Mvc;
using MimoBackend.API.Models;
using MimoBackend.API.Services;

namespace MimoBackend.API.Controllers;

[ApiController]
[Route("users")]
public class UserController
{
    private readonly ILogger<UserController> _logger;
    private readonly IAuthorizationService _authorizationService;

    public UserController(ILogger<UserController> logger, IAuthorizationService authorizationService)
    {
        _logger = logger;
        _authorizationService = authorizationService;
    }

    [HttpPost]
    [Route("authorize")]
    [Produces("application/json")]
    public IActionResult Authorize(Credentials credentials)
    {
        return _authorizationService.Authorize(credentials);
    }
}
