using Microsoft.AspNetCore.Mvc;
using MimoBackend.API.Models;
using MimoBackend.API.Models.DatabaseObjects;
using MimoBackend.API.Repositories;

namespace MimoBackend.API.Services;

public interface IUserAchievementService
{
    IEnumerable<UserAchievement> GetUserAchievements(string username);
    IEnumerable<UserAchievement> GetCompletedUserAchievements(string username);
    IEnumerable<UserAchievement> GetOngoingUserAchievements(string username);
    void UpdateLessonUserAchievement(User user);
    void UpdateChapterUserAchievement(User user);
    void UpdateCourseUserAchievement(Course course, User user);
}

public class UserAchievementService : BaseService, IUserAchievementService
{
    private readonly IUserService _userService;
    private readonly IAchievementService _achievementService;
    private readonly IUserAchievementRepository _userAchievementRepository;

    public UserAchievementService(
        IUserService userService, 
        IAchievementService achievementService,
        IUserAchievementRepository userAchievementRepository)
    {
        _userService = userService;
        _achievementService = achievementService;
        _userAchievementRepository = userAchievementRepository;
    }

    public IEnumerable<UserAchievement> GetUserAchievements(string username)
    {
        var user = _userService.GetUserBy(username);
        if (UserNotFound(user)) 
            return [];
        
        return _userAchievementRepository.GetUserAchievements(user);
    }

    public IEnumerable<UserAchievement> GetCompletedUserAchievements(string username)
    {
        var user = _userService.GetUserBy(username);
        if (UserNotFound(user)) 
            return [];
        
        return _userAchievementRepository.GetCompletedUserAchievements(user);
    }

    public IEnumerable<UserAchievement> GetOngoingUserAchievements(string username)
    {
        var user = _userService.GetUserBy(username);
        if (UserNotFound(user)) 
            return [];
        
        return _userAchievementRepository.GetUserAchievementsInProgress(user);
    }

    public void UpdateLessonUserAchievement(User user)
    {
        var achievements = _achievementService.GetAchievementsOfType(AchievementType.CompletedLessons)
            .ToList();
        UpdateUserAchievements(user, achievements);
    }

    public void UpdateChapterUserAchievement(User user)
    {
        var achievements = _achievementService.GetAchievementsOfType(AchievementType.CompletedChapters)
            .ToList();
        UpdateUserAchievements(user, achievements);
    }

    public void UpdateCourseUserAchievement(Course course, User user)
    {
        var achievements = _achievementService.GetAchievementsOfType(AchievementType.CompletedCourses)
            .Where(x => x.Name.Contains(course.Name))
            .ToList();
        UpdateUserAchievements(user, achievements);
    }
    
    private void UpdateUserAchievements(User user, IEnumerable<Achievement> achievements)
    {
        foreach (var achievement in achievements)
        {
            var userAchievement =
                _userAchievementRepository.GetUserAchievementByAchievementUserAndCompletion(achievement, user, false);

            if (userAchievement is null)
            {
                userAchievement = NewUserAchievementProgress(achievement, user);
                _userAchievementRepository.CreateUserAchievement(userAchievement);
            }
            UpdateUserAchievement(userAchievement, achievement);
            _userAchievementRepository.UpdateUserAchievement(userAchievement);
        }
    }
    
    private static UserAchievement NewUserAchievementProgress(Achievement achievement, User user) 
        => new()
        {
            UserUsername = user.Username,
            User = user,
            AchievementId = achievement.Id,
            Achievement = achievement
        };

    private static void UpdateUserAchievement(UserAchievement userAchievement, Achievement achievement)
    {
        userAchievement.Progress++;
        userAchievement.Completed = userAchievement.Progress == achievement.CompletionRequirements;
    }
}