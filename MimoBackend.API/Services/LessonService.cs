using Microsoft.AspNetCore.Mvc;
using MimoBackend.API.Models.DatabaseObjects;
using MimoBackend.API.Repositories;

namespace MimoBackend.API.Services;

public interface ILessonService
{
    Lesson? GetLessonBy(int lessonId);
    IActionResult GetLessons(string username);
}

public class LessonService : BaseService, ILessonService
{
    private readonly ILessonRepository _lessonRepository;

    public LessonService(ILessonRepository lessonRepository)
    {
        _lessonRepository = lessonRepository;
    }

    public Lesson? GetLessonBy(int lessonId)
    {
        throw new NotImplementedException();
    }

    public IActionResult GetLessons(string username)
    {
        var lessons = _lessonRepository.GetLessons();
        return BuildResponse(StatusCodes.Status200OK, lessons);
    }
}