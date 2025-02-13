using Microsoft.AspNetCore.Mvc;
using MimoBackend.API.Models;
using MimoBackend.API.Models.DatabaseObjects;
using MimoBackend.API.Repositories;

namespace MimoBackend.API.Services;

public interface ILessonProgressService
{
    Task<IActionResult> UpdateLesson(string lessonId, LessonUpdate lessonUpdate, string username);
    Task<IActionResult> StartLesson(string lessonId, DateTime date, string username);
    Task<IActionResult> CompleteLesson(string lessonId, DateTime date, string username);
}

public class LessonProgressService : BaseService, ILessonProgressService
{
    private readonly ILessonProgressRepository _lessonProgressRepository;

    public LessonProgressService(ILessonProgressRepository lessonProgressRepository)
    {
        _lessonProgressRepository = lessonProgressRepository;
    }

    public async Task<IActionResult> UpdateLesson(string lessonId, LessonUpdate lessonUpdate, string username)
    {
        var lessonProgress = await _lessonProgressRepository.UpdateLesson(lessonId, lessonUpdate, username);
        return LessonNotFound(lessonProgress) ? 
            BuildResponse(StatusCodes.Status404NotFound) : 
            BuildResponse(StatusCodes.Status200OK, lessonProgress);
    }

    public async Task<IActionResult> StartLesson(string lessonId, DateTime date, string username)
    {
        var lessonProgress = await _lessonProgressRepository.StartLesson(lessonId, date, username);
        return LessonNotFound(lessonProgress) ? 
            BuildResponse(StatusCodes.Status404NotFound) : 
            BuildResponse(StatusCodes.Status200OK, lessonProgress);
    }

    public async Task<IActionResult> CompleteLesson(string lessonId, DateTime date, string username)
    {
        var lessonProgress = await _lessonProgressRepository.CompleteLesson(lessonId, date, username);
        return LessonNotFound(lessonProgress) ? 
            BuildResponse(StatusCodes.Status404NotFound) : 
            BuildResponse(StatusCodes.Status200OK, lessonProgress);
    }
    
    private static bool LessonNotFound(LessonProgress? lessonProgress) 
        => lessonProgress is null;
}