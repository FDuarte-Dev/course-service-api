using Microsoft.AspNetCore.Mvc;
using MimoBackend.API.Services;

namespace MimoBackend.API.Controllers;

[ApiController]
[Route("lessons")]
public class LessonController
{
    private readonly ILessonService _lessonService;

    public LessonController(ILessonService lessonService)
    {
        _lessonService = lessonService;
    }
    
    [HttpGet]
    [Route("")]
    [Produces("application/json")]
    public IActionResult GetLessons([FromQuery(Name = "user")] string username)
    {
        return _lessonService.GetLessons(username);
    }
}
