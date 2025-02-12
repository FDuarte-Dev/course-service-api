using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
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
}