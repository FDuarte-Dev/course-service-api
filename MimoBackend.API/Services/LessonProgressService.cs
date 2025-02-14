using Microsoft.AspNetCore.Mvc;
using MimoBackend.API.Models;
using MimoBackend.API.Models.DatabaseObjects;
using MimoBackend.API.Repositories;

namespace MimoBackend.API.Services;

public interface ILessonProgressService
{
    IActionResult UpdateLesson(string lessonId, LessonUpdate lessonUpdate, string username);
    IActionResult StartLesson(string lessonId, DateTime startTime, string username);
    IActionResult CompleteLesson(string lessonId, DateTime completionTime, string username);
}

public class LessonProgressService : BaseService, ILessonProgressService
{
    private readonly ILessonRepository _lessonRepository;
    private readonly IUserRepository _userRepository;
    private readonly ILessonProgressRepository _lessonProgressRepository;

    public LessonProgressService(ILessonRepository lessonRepository,
        IUserRepository userRepository,
        ILessonProgressRepository lessonProgressRepository)
    {
        _lessonRepository = lessonRepository;
        _userRepository = userRepository;
        _lessonProgressRepository = lessonProgressRepository;
    }

    public IActionResult UpdateLesson(string lessonId, LessonUpdate lessonUpdate, string username)
    {
        var lesson = _lessonRepository.GetLessonBy(lessonId);
        if (LessonNotFoundReturns(lesson, out var lessonNotFound)) 
            return lessonNotFound;

        var user = _userRepository.GetUserBy(username);
        if (UserNotFoundReturns(user, out var userNotFound)) 
            return userNotFound;

        var lessonProgress = CreateLessonProgress(lesson!, user!, lessonUpdate.StartTime, lessonUpdate.CompletionTime);
        lessonProgress = _lessonProgressRepository.AddLessonProgress(lessonProgress);
        
        return BuildResponse(StatusCodes.Status200OK, lessonProgress);
    }

    public IActionResult StartLesson(string lessonId, DateTime startTime, string username)
    {
        var lesson = _lessonRepository.GetLessonBy(lessonId);
        if (LessonNotFoundReturns(lesson, out var lessonNotFound)) 
            return lessonNotFound;

        var user = _userRepository.GetUserBy(username);
        if (UserNotFoundReturns(user, out var userNotFound)) 
            return userNotFound;
        
        var lessonProgress = _lessonProgressRepository.FindByLessonUserAndCompletion(lesson, user, false);
        lessonProgress = lessonProgress is null ?
            _lessonProgressRepository.AddLessonProgress(CreateLessonProgress(lesson!, user!, startTime)) :
            _lessonProgressRepository.UpdateLessonProgressStartTime(lessonProgress.Id, startTime);
        return BuildResponse(StatusCodes.Status200OK, lessonProgress);
    }

    public IActionResult CompleteLesson(string lessonId, DateTime completionTime, string username)
    {
        var lesson = _lessonRepository.GetLessonBy(lessonId);
        if (LessonNotFoundReturns(lesson, out var lessonNotFound)) 
            return lessonNotFound;

        var user = _userRepository.GetUserBy(username);
        if (UserNotFoundReturns(user, out var userNotFound)) 
            return userNotFound;
        
        var lessonProgress = _lessonProgressRepository.FindByLessonUserAndCompletion(lesson, user, false);
        if (LessonProgressNotFoundReturns(lessonProgress, out var lessonProgressNotFound)) 
            return lessonProgressNotFound;
        lessonProgress = _lessonProgressRepository.UpdateLessonProgressCompletionTime(lessonProgress!.Id, completionTime);
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
}