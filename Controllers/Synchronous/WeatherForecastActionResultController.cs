using CacheAspectUniversalWrapper.Cache.Attributes;
using CacheAspectUniversalWrapper.Extensions;
using CacheAspectUniversalWrapper.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CacheAspectUniversalWrapper.Controllers.Synchronous;

[ApiController]
[Route("api")]
public class WeatherForecastActionResultController : ControllerBase
{
    private readonly IWeatherForecastService _service;

    public WeatherForecastActionResultController(IWeatherForecastService service)
    {
        _service = service;
    }

    [HttpGet("action-result/weather")]
    [TestCache("action-result/weather.json")]
    public ActionResult<IEnumerable<WeatherForecast>> GetWeatherForecast(
        [FromQuery] bool error = false)
    {
        return _service.GetForecast(error).HandleFailuresOrOk();
    }
}
