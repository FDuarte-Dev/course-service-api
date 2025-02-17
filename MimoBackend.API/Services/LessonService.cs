using Microsoft.AspNetCore.Mvc;
using MimoBackend.API.Repositories;

namespace MimoBackend.API.Services;

public interface ILessonService
{
    IActionResult GetLessons(string username);
}

public class LessonService : BaseService, ILessonService
{
    private readonly ILessonRepository _lessonRepository;

    public LessonService(ILessonRepository lessonRepository)
    {
        _lessonRepository = lessonRepository;
    }

    public IActionResult GetLessons(string username)
    {
        var lessons = _lessonRepository.GetLessons();
        return BuildResponse(StatusCodes.Status200OK, lessons);
    }
}