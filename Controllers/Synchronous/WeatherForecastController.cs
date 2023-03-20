using CacheAspectUniversalWrapper.Cache.Attributes;
using CacheAspectUniversalWrapper.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CacheAspectUniversalWrapper.Controllers.Synchronous;

[ApiController]
[Route("api")]
public class WeatherForecastController : ControllerBase
{
    private readonly IWeatherForecastService _service;

    public WeatherForecastController(IWeatherForecastService service)
    {
        _service = service;
    }

    [HttpGet("weather")]
    [TestCache("weather.json")]
    public IEnumerable<WeatherForecast> GetWeatherForecast()
    {
        return _service.GetForecast().Right;
    }
}
