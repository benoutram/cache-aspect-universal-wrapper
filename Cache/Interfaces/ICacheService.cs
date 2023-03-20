namespace CacheAspectUniversalWrapper.Cache.Interfaces;

public interface ICacheService
{
    object? GetItem(string key, Type targetType);

    Task<object?> GetItemAsync(string key, Type targetType, int minDelay, int maxDelay);

    void SetItem<T>(string key, T value);

    Task SetItemAsync<T>(string key, T value, int minDelay, int maxDelay);
}
