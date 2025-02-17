using MimoBackend.API.Persistence;

namespace MimoBackend.API.Repositories;

public interface IUserAchievementRepository
{
}

public class UserAchievementRepository : IUserAchievementRepository
{
    private readonly AppDbContext _context;

    public UserAchievementRepository(AppDbContext context)
    {
        _context = context;
    }
}