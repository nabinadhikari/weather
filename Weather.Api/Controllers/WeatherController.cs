using Microsoft.AspNetCore.Mvc;
using Weather.Api.RateLimit;
using Weather.Contracts;
using Weather.Data;

namespace Weather.Api.Controllers;

[ApiController]
[Route("weather")]
public class WeatherController : ControllerBase
{
    private readonly ILogger<WeatherController> _logger;
    private readonly IWeatherService _service;

    public WeatherController(ILogger<WeatherController> logger,
        IWeatherService weatherService)
    {
        _logger = logger;
        _service = weatherService;
    }

    [HttpPost(Name = nameof(GetWeatherDetails))]
    [RateLimiterAttr(MaxRequests = 5, TimeWindow = 60*60)]
    public async Task<ActionResult<WeatherResponseDto>> GetWeatherDetails([FromBody] WeatherRequestDto dto)
    {
        if (string.IsNullOrEmpty(dto.City) || string.IsNullOrEmpty(dto.Country))
        {
            return new BadRequestResult();
        }
        var temp = await _service.GetWeatherForCityAsync(dto.City, dto.Country);
        if (temp.ErrorNo == 404) return new NotFoundResult();
        var response = new WeatherResponseDto
        {
            Result = temp.Message
        };
        return new OkObjectResult(response);
    }
}
