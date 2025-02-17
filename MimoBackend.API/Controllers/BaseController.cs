using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using MimoBackend.API.Models;
using Newtonsoft.Json;

namespace MimoBackend.API.Controllers;

public class BaseController
{
    protected static IActionResult BuildResponse<T>(T content) 
        => new ContentResult()
        {
            StatusCode = GetStatusCodeFrom(content),
            Content = JsonConvert.SerializeObject(content),
            ContentType = MediaTypeNames.Application.Json
        };

    private static int GetStatusCodeFrom(object content)
    {
        if (content.GetType() is NotFoundObject)
        {
            return StatusCodes.Status404NotFound;
        }

        return StatusCodes.Status200OK;
    }
}