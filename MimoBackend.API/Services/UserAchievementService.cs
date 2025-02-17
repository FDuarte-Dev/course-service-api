using Microsoft.AspNetCore.Mvc;
using MimoBackend.API.Models.DatabaseObjects;
using MimoBackend.API.Repositories;

namespace MimoBackend.API.Services;

public interface IUserAchievementService
{
    IActionResult GetUserAchievements(string username);
    IActionResult GetCompletedUserAchievements(string username);
    IActionResult GetUserAchievementsInProgress(string username);
    void UpdateLessonUserAchievement(Lesson lesson, User user);
    void UpdateChapterUserAchievement(Chapter chapter, User user);
    void UpdateCourseUserAchievement(Course course, User user);
}

public class UserAchievementService : IUserAchievementService
{
    private readonly IUserRepository _userRepository;
    private readonly IAchievementRepository _achievementRepository;
    private readonly IUserAchievementRepository _userAchievementRepository;

    public UserAchievementService(
        IUserRepository userRepository, 
        IAchievementRepository achievementRepository, 
        IUserAchievementRepository userAchievementRepository)
    {
        _userRepository = userRepository;
        _achievementRepository = achievementRepository;
        _userAchievementRepository = userAchievementRepository;
    }

    public IActionResult GetUserAchievements(string username)
    {
        throw new NotImplementedException();
    }

    public IActionResult GetCompletedUserAchievements(string username)
    {
        throw new NotImplementedException();
    }

    public IActionResult GetUserAchievementsInProgress(string username)
    {
        throw new NotImplementedException();
    }

    public void UpdateLessonUserAchievement(Lesson lesson, User user)
    {
        throw new NotImplementedException();
    }

    public void UpdateChapterUserAchievement(Chapter chapter, User user)
    {
        throw new NotImplementedException();
    }

    public void UpdateCourseUserAchievement(Course course, User user)
    {
        throw new NotImplementedException();
    }
}