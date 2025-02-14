using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
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
    
    protected static bool UserNotFoundReturns(User? user, out IActionResult actionResult)
    {
        actionResult = null!;
        if (UserNotFound())
        {
            {
                actionResult = BuildResponse(StatusCodes.Status404NotFound, "User not found");
                return true;
            }
        }

        return false;
        
        bool UserNotFound() 
            => user is null;
    }
    
    protected static bool LessonNotFoundReturns(Lesson? lesson, out IActionResult actionResult)
    {
        actionResult = null!;
        if (LessonNotFound())
        {
            {
                actionResult = BuildResponse(StatusCodes.Status404NotFound, "Lesson not found");
                return true;
            }
        }

        return false;
        
        bool LessonNotFound() 
            => lesson is null;
    }
    
    protected static bool LessonProgressNotFoundReturns(LessonProgress? lessonProgress, out IActionResult actionResult)
    {
        actionResult = null!;
        if (LessonProgressNotFound())
        {
            {
                actionResult = BuildResponse(StatusCodes.Status404NotFound, "Lesson progress not found");
                return true;
            }
        }

        return false;
        
        bool LessonProgressNotFound() 
            => lessonProgress is null;
    }
}