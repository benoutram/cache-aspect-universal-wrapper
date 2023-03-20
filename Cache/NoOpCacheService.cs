using CacheAspectUniversalWrapper.Cache.Interfaces;

namespace CacheAspectUniversalWrapper.Cache;

public class NoOpCacheService : ICacheService
{
    private readonly ILogger<NoOpCacheService> _logger;

    public NoOpCacheService(ILogger<NoOpCacheService> logger)
    {
        _logger = logger;
    }

    public object? GetItem(string key, Type targetType)
    {
        _logger.LogInformation("{service}.{method} returning", nameof(NoOpCacheService), nameof(GetItem));
        return default;
    }

    public Task<object?> GetItemAsync(string key, Type targetType, int minDelay, int maxDelay)
    {
        var random = new Random();
        var delay = random.Next(minDelay, maxDelay);

        _logger.LogInformation("{service}.{method} starting (delay={delay})",
            nameof(NoOpCacheService),
            nameof(GetItemAsync),
            delay);

        return Task.Run(async () =>
        {
            await Task.Delay(delay);

            _logger.LogInformation("{service}.{method} returning (delay={delay})",
                nameof(NoOpCacheService),
                nameof(GetItemAsync),
                delay);

            return (object?) default;
        });
    }

    public void SetItem<T>(string key, T value)
    {
        _logger.LogInformation("{service}.{method} returning", nameof(NoOpCacheService), nameof(SetItemAsync));
    }

    public Task SetItemAsync<T>(string key, T value, int minDelay, int maxDelay)
    {
        var random = new Random();
        var delay = random.Next(minDelay, maxDelay);

        _logger.LogInformation("{service}.{method} starting (delay={delay})",
            nameof(NoOpCacheService),
            nameof(SetItemAsync),
            delay);

        return Task.Run(async () =>
        {
            await Task.Delay(delay);

            _logger.LogInformation("{service}.{method} returning (delay={delay})",
                nameof(NoOpCacheService),
                nameof(SetItemAsync),
                delay);
        });
    }
}
