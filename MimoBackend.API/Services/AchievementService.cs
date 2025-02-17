using Microsoft.AspNetCore.Mvc;
using MimoBackend.API.Repositories;

namespace MimoBackend.API.Services;

public interface IAchievementService
{
    IActionResult GetAchievements(string username);
}

public class AchievementService : BaseService, IAchievementService
{
    private readonly IAchievementRepository _achievementRepository;

    public AchievementService(IAchievementRepository achievementRepository)
    {
        _achievementRepository = achievementRepository;
    }

    public IActionResult GetAchievements(string username)
    {
        return BuildResponse(StatusCodes.Status200OK, _achievementRepository.GetAchievements());
    }
}