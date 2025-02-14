using Microsoft.AspNetCore.Mvc;
using MimoBackend.API.Models;
using MimoBackend.API.Services;

namespace MimoBackend.API.Controllers;

[ApiController]
[Route("lessons/{lessonId}/progress")]
public class LessonProgressController
{
    private readonly ILessonProgressService _lessonProgressService;

    public LessonProgressController(ILessonProgressService lessonProgressService)
    {
        _lessonProgressService = lessonProgressService;
    }

    [HttpPost]
    [Route("update")]
    [Produces("application/json")]
    public IActionResult UpdateLesson(int lessonId, LessonUpdate lessonUpdate, [FromQuery(Name = "user")] string username)
    {
        return _lessonProgressService.UpdateLesson(lessonId, lessonUpdate, username);
    }
    
    [HttpPost]
    [Route("start")]
    [Produces("application/json")]
    public IActionResult StartLesson(int lessonId, DateTime date, [FromQuery(Name = "user")] string username)
    {
        return _lessonProgressService.StartLesson(lessonId, date, username);
    }
    
    [HttpPut]
    [Route("complete")]
    [Produces("application/json")]
    public IActionResult CompleteLesson(int lessonId, DateTime date, [FromQuery(Name = "user")] string username)
    {
        return _lessonProgressService.CompleteLesson(lessonId, date, username);
    }
}