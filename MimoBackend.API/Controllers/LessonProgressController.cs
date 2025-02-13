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
    public async Task<IActionResult> UpdateLesson(string lessonId, LessonUpdate lessonUpdate, [FromQuery(Name = "user")] string username)
    {
        return await _lessonProgressService.UpdateLesson(lessonId, lessonUpdate, username);
    }
    
    [HttpPost]
    [Route("start")]
    [Produces("application/json")]
    public async Task<IActionResult> StartLesson(string lessonId, DateTime date, [FromQuery(Name = "user")] string username)
    {
        return await _lessonProgressService.StartLesson(lessonId, date, username);
    }
    
    [HttpPut]
    [Route("complete")]
    [Produces("application/json")]
    public async Task<IActionResult> CompleteLesson(string lessonId, DateTime date, [FromQuery(Name = "user")] string username)
    {
        return await _lessonProgressService.CompleteLesson(lessonId, date, username);
    }
}