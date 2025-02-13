using Microsoft.AspNetCore.Mvc;
using MimoBackend.API.Models;

namespace MimoBackend.API.Services;

public interface ILessonProgressService
{
    Task<IActionResult> UpdateLesson(string lessonId, LessonUpdate lessonUpdate, string username);
    Task<IActionResult> StartLesson(string lessonId, DateTime date, string username);
    Task<IActionResult> CompleteLesson(string lessonId, DateTime date, string username);
}

public class LessonProgressService : BaseService, ILessonProgressService
{
    public Task<IActionResult> UpdateLesson(string lessonId, LessonUpdate lessonUpdate, string username)
    {
        throw new NotImplementedException();
    }

    public Task<IActionResult> StartLesson(string lessonId, DateTime date, string username)
    {
        throw new NotImplementedException();
    }

    public Task<IActionResult> CompleteLesson(string lessonId, DateTime date, string username)
    {
        throw new NotImplementedException();
    }
}