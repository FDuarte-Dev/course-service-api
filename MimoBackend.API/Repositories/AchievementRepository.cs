using MimoBackend.API.Models;
using MimoBackend.API.Models.DatabaseObjects;
using MimoBackend.API.Persistence;

namespace MimoBackend.API.Repositories;

public interface IAchievementRepository
{
    IEnumerable<Achievement> GetAchievements();
    IEnumerable<Achievement> GetAchievementsOfType(AchievementType achievementType);
}

public class AchievementRepository : IAchievementRepository
{
    private readonly AppDbContext _context;

    public AchievementRepository(AppDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Achievement> GetAchievements()
    {
        return _context.Achievements.AsEnumerable();
    }

    public IEnumerable<Achievement> GetAchievementsOfType(AchievementType achievementType)
    {
        return _context.Achievements.Where(x => x.Type == achievementType);
    }
}