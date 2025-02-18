using MimoBackend.API.Models;
using MimoBackend.API.Models.DatabaseObjects;
using MimoBackend.API.Repositories;

namespace MimoBackend.API.Services;

public interface IAchievementService
{
    IEnumerable<Achievement> GetAchievements();
    IEnumerable<Achievement> GetAchievementsOfType(AchievementType achievementType);
}

public class AchievementService : BaseService, IAchievementService
{
    private readonly IAchievementRepository _achievementRepository;

    public AchievementService(IAchievementRepository achievementRepository)
    {
        _achievementRepository = achievementRepository;
    }

    public IEnumerable<Achievement> GetAchievements()
    {
        return _achievementRepository.GetAchievements();
    }

    public IEnumerable<Achievement> GetAchievementsOfType(AchievementType achievementType)
    {
        return _achievementRepository.GetAchievementsOfType(achievementType);
    }
}