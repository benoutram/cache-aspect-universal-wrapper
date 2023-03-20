using CacheAspectUniversalWrapper.Cache.Attributes;
using CacheAspectUniversalWrapper.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CacheAspectUniversalWrapper.Controllers.Asynchronous;

[ApiController]
[Route("api")]
public class WeatherForecastAsyncController : ControllerBase
{
    private readonly IWeatherForecastService _service;

    public WeatherForecastAsyncController(IWeatherForecastService service)
    {
        _service = service;
    }

    [HttpGet("async/weather")]
    [TestCache("async/weather.json")]
    public async Task<IEnumerable<WeatherForecast>> GetWeatherForecastAsync(
        [FromQuery] bool delayed = false)
    {
        return delayed ? (await _service.GetForecastDelayedAsync()).Right :
            (await _service.GetForecastAsync()).Right;
    }
}
