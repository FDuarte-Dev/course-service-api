using Microsoft.AspNetCore.Mvc;

namespace MimoBackend.API.Controllers;

[ApiController]
[Route("health")]
public class HealthCheckController
{
    private readonly ILogger<HealthCheckController> _logger;

    public HealthCheckController(ILogger<HealthCheckController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    [Route("status")]
    public IActionResult Status() 
        => new OkObjectResult(true);
}