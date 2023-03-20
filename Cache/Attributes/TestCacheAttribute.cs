using CacheAspectUniversalWrapper.Cache.Interfaces;

namespace CacheAspectUniversalWrapper.Cache.Attributes;

public class TestCacheAttribute : BaseCacheAttribute
{
    // Set during startup
    public static ICacheService CacheService { get; set; } = null!;

    /// <summary>
    /// Minimum delay to introduce when setting/getting item in the cached service in milliseconds.
    /// </summary>
    public int MinDelay = 1;

    /// <summary>
    /// Maximum delay to introduce when setting/getting an item in the cached service in milliseconds.
    /// </summary>
    public int MaxDelay = 101;

    public TestCacheAttribute(string key) : base(key)
    {
    }

    protected override object? GetItem(string key, Type targetType)
    {
        return CacheService.GetItem(key, targetType);
    }

    protected override Task<object?> GetItemAsync(string key, Type targetType)
    {
        return CacheService.GetItemAsync(key, targetType, MinDelay, MaxDelay);
    }

    protected override void SetItem<T>(string key, T value)
    {
        CacheService.SetItem(key, value);
    }

    protected override Task SetItemAsync<T>(string key, T value)
    {
        return CacheService.SetItemAsync(key, value, MinDelay, MaxDelay);
    }
}
