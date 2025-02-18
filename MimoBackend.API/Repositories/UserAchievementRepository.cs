using System.Collections;
using MimoBackend.API.Models.DatabaseObjects;
using MimoBackend.API.Persistence;

namespace MimoBackend.API.Repositories;

public interface IUserAchievementRepository
{
    IEnumerable<UserAchievement> GetUserAchievements(User user);
    IEnumerable<UserAchievement> GetCompletedUserAchievements(User user);
    IEnumerable<UserAchievement> GetUserAchievementsInProgress(User user);
    UserAchievement? GetUserAchievementByAchievementUserAndCompletion(Achievement achievement, User user, bool completed);
    UserAchievement CreateUserAchievement(UserAchievement userAchievement);
    UserAchievement UpdateUserAchievement(UserAchievement userAchievement);
}

public class UserAchievementRepository : IUserAchievementRepository
{
    private readonly AppDbContext _context;

    public UserAchievementRepository(AppDbContext context)
    {
        _context = context;
    }

    public IEnumerable<UserAchievement> GetUserAchievements(User user) 
        => _context.UserAchievements.Where(x => x.UserUsername == user.Username);

    public IEnumerable<UserAchievement> GetCompletedUserAchievements(User user) 
        => _context.UserAchievements.Where(x =>
            x.UserUsername == user.Username &&
            x.Completed);

    public IEnumerable<UserAchievement> GetUserAchievementsInProgress(User user)
        => _context.UserAchievements.Where(x =>
            x.UserUsername == user.Username &&
            !x.Completed);

    public UserAchievement? GetUserAchievementByAchievementUserAndCompletion(Achievement achievement, User user, bool completed)
    {
        return _context.UserAchievements
            .FirstOrDefault(x => 
                x.AchievementId == achievement.Id && 
                x.UserUsername == user.Username &&
                x.Completed == completed);
    }

    public UserAchievement CreateUserAchievement(UserAchievement userAchievement)
    {
        var entry = _context.UserAchievements.Add(userAchievement);
        _context.SaveChanges();
        return entry.Entity;
    }

    public UserAchievement UpdateUserAchievement(UserAchievement userAchievement)
    {
        var entry = _context.UserAchievements.Update(userAchievement);
        _context.SaveChanges();
        return entry.Entity;
    }
}