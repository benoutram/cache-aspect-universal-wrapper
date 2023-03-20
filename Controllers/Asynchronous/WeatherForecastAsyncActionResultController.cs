using CacheAspectUniversalWrapper.Cache.Attributes;
using CacheAspectUniversalWrapper.Extensions;
using CacheAspectUniversalWrapper.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CacheAspectUniversalWrapper.Controllers.Asynchronous;

[ApiController]
[Route("api")]
public class WeatherForecastAsyncActionResultController : ControllerBase
{
    private readonly IWeatherForecastService _service;

    public WeatherForecastAsyncActionResultController(IWeatherForecastService service)
    {
        _service = service;
    }

    [HttpGet("async/action-result/weather")]
    [TestCache("async/action-result/weather.json")]
    public async Task<ActionResult<IEnumerable<WeatherForecast>>> GetWeatherForecastAsync(
        [FromQuery] bool delayed = false,
        [FromQuery] bool error = false)
    {
        return (await (delayed ? _service.GetForecastDelayedAsync(error) : _service.GetForecastAsync(error)))
            .HandleFailuresOrOk();
    }
}
