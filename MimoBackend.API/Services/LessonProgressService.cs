using Microsoft.AspNetCore.Mvc;
using MimoBackend.API.Models;
using MimoBackend.API.Models.DatabaseObjects;
using MimoBackend.API.Repositories;

namespace MimoBackend.API.Services;

public interface ILessonProgressService
{
    IActionResult UpdateLesson(string lessonId, LessonUpdate lessonUpdate, string username);
    IActionResult StartLesson(string lessonId, DateTime startTime, string username);
    IActionResult CompleteLesson(string lessonId, DateTime completionDate, string username);
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

        var lessonProgress = new LessonProgress()
        {
            LessonId = lesson.Id,
            Lesson = lesson,
            UserUsername = user.Username,
            User = user,
            StartTime = lessonUpdate.StartTime,
            CompletionTime = lessonUpdate.CompletionTime
        };
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
        
        var lessonProgresses = _lessonProgressRepository.FindByLessonUserAndCompletion(lesson, user, false);
        // TODO: WIP
        LessonProgress lessonProgress = new LessonProgress();
        if (!lessonProgresses.Any())
        {
            lessonProgress = _lessonProgressRepository.AddLessonProgress(lessonProgress);
        }
        else
        {
            lessonProgress = _lessonProgressRepository.UpdateLessonProgress(lessonProgress);
            
        }
        return BuildResponse(StatusCodes.Status200OK, lessonProgress);
    }

    public IActionResult CompleteLesson(string lessonId, DateTime completionDate, string username)
    {
        var lesson = _lessonRepository.GetLessonBy(lessonId);
        if (LessonNotFoundReturns(lesson, out var lessonNotFound)) 
            return lessonNotFound;

        var user = _userRepository.GetUserBy(username);
        if (UserNotFoundReturns(user, out var userNotFound)) 
            return userNotFound;
        
        // TODO: WIP
        var lessonProgress = _lessonProgressRepository.FindByLessonAndUser(lesson, user);
        // lessonProgress = _lessonProgressRepository.UpdateLessonProgress(lessonProgress);
        return BuildResponse(StatusCodes.Status200OK, lessonProgress);
    }
}

public interface ILessonRepository
{
    Lesson? GetLessonBy(string lessonId);
}