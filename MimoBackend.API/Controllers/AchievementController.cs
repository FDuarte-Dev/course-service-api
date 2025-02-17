using Microsoft.AspNetCore.Mvc;
using MimoBackend.API.Services;

namespace MimoBackend.API.Controllers;

[ApiController]
[Route("achievements")]
public class AchievementController
{
    private readonly IAchievementService _achievementService;

    public AchievementController(IAchievementService achievementService)
    {
        _achievementService = achievementService;
    }
    
    [HttpGet]
    [Route("")]
    [Produces("application/json")]
    public IActionResult GetAchievements([FromQuery(Name = "user")] string username)
    {
        return _achievementService.GetAchievements(username);
    }
}