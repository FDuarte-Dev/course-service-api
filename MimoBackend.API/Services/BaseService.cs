using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using MimoBackend.API.Models;
using MimoBackend.API.Models.DatabaseObjects;
using Newtonsoft.Json;

namespace MimoBackend.API.Services;

public abstract class BaseService
{
    protected static IActionResult BuildResponse(int statusCode) 
        => new ContentResult()
        {
            StatusCode = statusCode,
        };

    protected static IActionResult BuildResponse<T>(int statusCode, T content) 
        => new ContentResult()
        {
            StatusCode = statusCode,
            Content = JsonConvert.SerializeObject(content),
            ContentType = MediaTypeNames.Application.Json
        };
    
    protected static bool UserNotFound(User? user) 
        => user is NotFoundObject;

    protected static bool LessonNotFound(Lesson? lesson) 
        => lesson is NotFoundObject;

    protected static bool LessonProgressNotFound(LessonProgress? lessonProgress) 
        => lessonProgress is NotFoundObject;
}