using CacheAspectUniversalWrapper.Model;
using Microsoft.AspNetCore.Mvc;

namespace CacheAspectUniversalWrapper.Services.Interfaces;

public interface IWeatherForecastService
{
    Either<ActionResult, IEnumerable<WeatherForecast>> GetForecast(bool error = false);

    Task<Either<ActionResult, IEnumerable<WeatherForecast>>> GetForecastAsync(bool error = false);

    Task<Either<ActionResult, IEnumerable<WeatherForecast>>> GetForecastDelayedAsync(bool error = false,
        int minDelay = 1,
        int maxDelay = 101);
}
