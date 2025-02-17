using Microsoft.AspNetCore.Mvc;
using MimoBackend.API.Models;
using MimoBackend.API.Models.DatabaseObjects;
using MimoBackend.API.Repositories;

namespace MimoBackend.API.Services;

public interface ILessonProgressService
{
    IActionResult UpdateLesson(int lessonId, LessonUpdate lessonUpdate, string username);
    IActionResult StartLesson(int lessonId, DateTime startTime, string username);
    IActionResult CompleteLesson(int lessonId, DateTime completionTime, string username);
}

public class LessonProgressService : BaseService, ILessonProgressService
{
    private readonly IChapterService _chapterService;
    private readonly ICourseService _courseService;
    private readonly IUserAchievementService _userAchievementService;
    private readonly ILessonService _lessonService;
    private readonly IUserService _userService;
    private readonly ILessonProgressRepository _lessonProgressRepository;

    public LessonProgressService(
        IChapterService chapterService,
        ICourseService courseService,
        IUserAchievementService userAchievementService,
        ILessonService lessonService,
        IUserService userService,
        ILessonProgressRepository lessonProgressRepository)
    {
        _chapterService = chapterService;
        _courseService = courseService;
        _userAchievementService = userAchievementService;
        _lessonService = lessonService;
        _userService = userService;
        _lessonProgressRepository = lessonProgressRepository;
    }

    public IActionResult UpdateLesson(int lessonId, LessonUpdate lessonUpdate, string username)
    {
        var lesson = _lessonService.GetLessonBy(lessonId);
        if (LessonNotFoundReturns(lesson, out var lessonNotFound)) 
            return lessonNotFound;

        var user = _userService.GetUserBy(username);
        if (UserNotFoundReturns(user, out var userNotFound)) 
            return userNotFound;

        var lessonProgress = CreateLessonProgress(lesson!, user!, lessonUpdate.StartTime, lessonUpdate.CompletionTime);
        lessonProgress = _lessonProgressRepository.AddLessonProgress(lessonProgress);

        UpdateLessonUserAchievement(lesson!, user!);
        
        return BuildResponse(StatusCodes.Status200OK, lessonProgress);
    }

    public IActionResult StartLesson(int lessonId, DateTime startTime, string username)
    {
        var lesson = _lessonService.GetLessonBy(lessonId);
        if (LessonNotFoundReturns(lesson, out var lessonNotFound)) 
            return lessonNotFound;

        var user = _userService.GetUserBy(username);
        if (UserNotFoundReturns(user, out var userNotFound)) 
            return userNotFound;
        
        var lessonProgress = _lessonProgressRepository.FindByLessonUserAndCompletion(lesson!, user!, false);
        lessonProgress = lessonProgress is null ?
            _lessonProgressRepository.AddLessonProgress(CreateLessonProgress(lesson!, user!, startTime)) :
            _lessonProgressRepository.UpdateLessonProgressStartTime(lessonProgress.Id, startTime);
        return BuildResponse(StatusCodes.Status200OK, lessonProgress);
    }

    public IActionResult CompleteLesson(int lessonId, DateTime completionTime, string username)
    {
        var lesson = _lessonService.GetLessonBy(lessonId);
        if (LessonNotFoundReturns(lesson, out var lessonNotFound)) 
            return lessonNotFound;

        var user = _userService.GetUserBy(username);
        if (UserNotFoundReturns(user, out var userNotFound)) 
            return userNotFound;
        
        var lessonProgress = _lessonProgressRepository.FindByLessonUserAndCompletion(lesson!, user!, false);
        if (LessonProgressNotFoundReturns(lessonProgress, out var lessonProgressNotFound)) 
            return lessonProgressNotFound;
        lessonProgress = _lessonProgressRepository.UpdateLessonProgressCompletionTime(lessonProgress!.Id, completionTime);
        
        UpdateLessonUserAchievement(lesson!, user!);
        
        return BuildResponse(StatusCodes.Status200OK, lessonProgress);
    }
    
    private static LessonProgress CreateLessonProgress(Lesson lesson, User user, DateTime startTime) 
        => CreateLessonProgress(lesson, user, startTime, null);
    
    private static LessonProgress CreateLessonProgress(Lesson lesson, User user, DateTime startTime, DateTime? completionTime) 
        => new()
        {
            LessonId = lesson.Id,
            Lesson = lesson,
            UserUsername = user.Username,
            User = user,
            StartTime = startTime,
            CompletionTime = completionTime
        };
    
    private void UpdateLessonUserAchievement(Lesson lesson, User user)
    {
        _userAchievementService.UpdateLessonUserAchievement(lesson, user);

        TryUpdateChapterUserAchievement(lesson, user);
    }

    private void TryUpdateChapterUserAchievement(Lesson lesson, User user)
    {
        var chapter = _chapterService.GetChapterBy(lesson.ChapterId);
        if (CompletedChapter(chapter, user))
        {
            _userAchievementService.UpdateChapterUserAchievement(chapter, user);

            TryUpdateCourseUserAchievement(user, chapter);
        }
    }

    private void TryUpdateCourseUserAchievement(User user, Chapter chapter)
    {
        var course = _courseService.GetCourseBy(chapter.CourseId);
        if (CompletedCourse(course, user))
        {
            _userAchievementService.UpdateCourseUserAchievement(course, user);
        }
    }

    private bool CompletedChapter(Chapter chapter, User user)
    {
        throw new NotImplementedException();
    }
    
    private bool CompletedCourse(Course course, User user)
    {
        throw new NotImplementedException();
    }
}