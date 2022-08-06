using Microsoft.AspNetCore.Mvc;

namespace Weather.Api.Controllers;

[ApiController]
[Route("weather")]
public class WeatherController : ControllerBase
{
    private readonly ILogger<WeatherController> _logger;

    public WeatherController(ILogger<WeatherController> logger)
    {
        _logger = logger;
    }
}
