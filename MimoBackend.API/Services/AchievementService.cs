using MimoBackend.API.Models.DatabaseObjects;
using MimoBackend.API.Repositories;

namespace MimoBackend.API.Services;

public interface IAchievementService
{
    IEnumerable<Achievement> GetAchievements(string username);
}

public class AchievementService : IAchievementService
{
    private readonly IAchievementRepository _achievementRepository;

    public AchievementService(IAchievementRepository achievementRepository)
    {
        _achievementRepository = achievementRepository;
    }

    public IEnumerable<Achievement> GetAchievements(string username)
    {
        return _achievementRepository.GetAchievements();
    }
}