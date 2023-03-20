using CacheAspectUniversalWrapper.Model;
using CacheAspectUniversalWrapper.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CacheAspectUniversalWrapper.Services;

public class WeatherForecastService : IWeatherForecastService
{
    private readonly ILogger<WeatherForecastService> _logger;

    private static readonly string[] Summaries =
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    public WeatherForecastService(ILogger<WeatherForecastService> logger)
    {
        _logger = logger;
    }

    public Either<ActionResult, IEnumerable<WeatherForecast>> GetForecast(bool error = false)
    {
        _logger.LogInformation("{service}.{method} returning",
            nameof(WeatherForecastService),
            nameof(GetForecast));

        return GenerateRandomForecast(error);
    }

    public Task<Either<ActionResult, IEnumerable<WeatherForecast>>> GetForecastAsync(bool error = false)
    {
        return Task.Run(() =>
        {
            _logger.LogInformation("{service}.{method} returning",
                nameof(WeatherForecastService),
                nameof(GetForecastAsync));

            return Task.FromResult(GenerateRandomForecast(error));
        });
    }

    public Task<Either<ActionResult, IEnumerable<WeatherForecast>>> GetForecastDelayedAsync(bool error = false,
        int minDelay = 1,
        int maxDelay = 101)
    {
        var random = new Random();
        var delay = random.Next(minDelay, maxDelay);

        _logger.LogInformation("{service}.{method} starting (delay={delay})",
            nameof(WeatherForecastService),
            nameof(GetForecastDelayedAsync),
            delay);

        return Task.Run(async () =>
        {
            await Task.Delay(delay);
            var result = GenerateRandomForecast(error);

            _logger.LogInformation("{service}.{method} returning (delay={delay})",
                nameof(WeatherForecastService),
                nameof(GetForecastDelayedAsync),
                delay);

            return result;
        });
    }

    private static Either<ActionResult, IEnumerable<WeatherForecast>> GenerateRandomForecast(bool error)
    {
        if (error)
        {
            return new NotFoundResult();
        }

        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }
}
