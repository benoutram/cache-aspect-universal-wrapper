using CacheAspectUniversalWrapper.Cache.Attributes;
using CacheAspectUniversalWrapper.Extensions;
using CacheAspectUniversalWrapper.Model;
using CacheAspectUniversalWrapper.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CacheAspectUniversalWrapper.Controllers.Asynchronous;

[ApiController]
[Route("api")]
public class WeatherForecastAsyncEitherController : ControllerBase
{
    private readonly IWeatherForecastService _service;

    public WeatherForecastAsyncEitherController(IWeatherForecastService service)
    {
        _service = service;
    }

    [HttpGet("async/either/weather")]
    public async Task<ActionResult<IEnumerable<WeatherForecast>>> GetWeatherForecastAsync(
        [FromQuery] bool delayed = false,
        [FromQuery] bool error = false)
    {
        return (await GetActualWeatherForecast(delayed: delayed, error: error))
            .HandleFailuresOrOk();
    }

    [TestCache("async/either/weather.json")]
    private Task<Either<ActionResult, IEnumerable<WeatherForecast>>> GetActualWeatherForecast(bool delayed,
        bool error)
    {
        return delayed ? _service.GetForecastDelayedAsync(error) : _service.GetForecastAsync(error);
    }
}
