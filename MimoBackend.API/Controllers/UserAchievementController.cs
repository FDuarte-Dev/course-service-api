using Microsoft.AspNetCore.Mvc;
using MimoBackend.API.Services;

namespace MimoBackend.API.Controllers;

[ApiController]
[Route("/user/achievements/")]
public class UserAchievementController: BaseController
{
    private readonly IUserAchievementService _userAchievementService;

    public UserAchievementController(IUserAchievementService userAchievementService)
    {
        _userAchievementService = userAchievementService;
    }
    
    [HttpGet]
    [Route("")]
    [Produces("application/json")]
    public IActionResult GetUserAchievements([FromQuery(Name = "user")] string username)
    {
        return BuildResponse(_userAchievementService.GetUserAchievements(username));
    }
    
    [HttpGet]
    [Route("completed")]
    [Produces("application/json")]
    public IActionResult GetCompletedUserAchievements([FromQuery(Name = "user")] string username)
    {
        return BuildResponse(_userAchievementService.GetCompletedUserAchievements(username));
    }
    
    [HttpGet]
    [Route("in-progress")]
    [Produces("application/json")]
    public IActionResult GetUserAchievementsInProgress([FromQuery(Name = "user")] string username)
    {
        return BuildResponse(_userAchievementService.GetOngoingUserAchievements(username));
    }
}