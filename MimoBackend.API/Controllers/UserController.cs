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

    public UserController(ILogger<UserController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    [Route("authorize")]
    [Produces("application/json")]
    public async Task<IActionResult> Authorize(Credentials credentials)
    {
        return await _authorizationService.Authorize(credentials);
    }
}
