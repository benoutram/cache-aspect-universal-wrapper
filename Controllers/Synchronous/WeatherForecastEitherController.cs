using CacheAspectUniversalWrapper.Cache.Attributes;
using CacheAspectUniversalWrapper.Extensions;
using CacheAspectUniversalWrapper.Model;
using CacheAspectUniversalWrapper.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CacheAspectUniversalWrapper.Controllers.Synchronous;

[ApiController]
[Route("api")]
public class WeatherForecastEitherController : ControllerBase
{
    private readonly IWeatherForecastService _service;

    public WeatherForecastEitherController(IWeatherForecastService service)
    {
        _service = service;
    }

    [HttpGet("either/weather")]
    public ActionResult<IEnumerable<WeatherForecast>> GetWeatherForecast(
        [FromQuery] bool error = false)
    {
        return GetActualWeatherForecast(error).HandleFailuresOrOk();
    }

    [TestCache("either/weather.json")]
    private Either<ActionResult, IEnumerable<WeatherForecast>> GetActualWeatherForecast(bool error)
    {
        return _service.GetForecast(error);
    }
}
