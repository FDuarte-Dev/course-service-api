using Microsoft.AspNetCore.Mvc;
using MimoBackend.API.Services;

namespace MimoBackend.API.Controllers;

[ApiController]
[Route("lessons")]
public class LessonController : BaseController
{
    private readonly ILessonService _lessonService;

    public LessonController(ILessonService lessonService)
    {
        _lessonService = lessonService;
    }
    
    [HttpGet]
    [Route("")]
    [Produces("application/json")]
    public IActionResult GetLessons()
    {
        return BuildResponse(_lessonService.GetLessons());
    }
}
