using AspectInjector.Broker;
using Aspects.Universal.Attributes;
using Aspects.Universal.Events;
using CacheAspectUniversalWrapper.Extensions;

namespace CacheAspectUniversalWrapper.Cache.Attributes;

/// <summary>
/// Base caching attribute that should be extended with specific
/// caching implementations e.g. blob storage, memory, Redis, etc.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
[Injection(typeof(CacheAspect), Inherited = true)]
public abstract class BaseCacheAttribute : BaseUniversalWrapperAttribute
{
    private readonly string _key;

    protected BaseCacheAttribute(string key)
    {
        _key = key;
    }

    protected override T WrapSync<T>(Func<object[], T> target, object[] args, AspectEventArgs eventArgs)
    {
        // May be using boxed types, and these may be nested, so we
        // recursively go through all the types to get the value
        // type that we're interested in getting/setting.
        var unboxedResultType = eventArgs.ReturnType.GetUnboxedResultTypePath().Last();

        var cachedResult = GetItem(_key, unboxedResultType);
        if (cachedResult?.TryBoxToResult(unboxedResultType, out cachedResult) == true)
        {
            return (T) cachedResult;
        }

        try
        {
            var result = target(args);

            if (!result.TryUnboxResult(out cachedResult) || cachedResult is null)
            {
                return result;
            }

            SetItem(_key, cachedResult);

            return result;
        }
        catch (Exception exception)
        {
            return OnException<T>(eventArgs, exception);
        }
    }

    protected override async Task<T> WrapAsync<T>(Func<object[], Task<T>> target, object[] args,
        AspectEventArgs eventArgs)
    {
        // May be using boxed types, and these may be nested, so we
        // recursively go through all the types to get the value
        // type that we're interested in getting/setting.
        var unboxedResultType = eventArgs.ReturnType.GetUnboxedResultTypePath().Last();

        var cachedResult = await GetItemAsync(_key, unboxedResultType);

        if (cachedResult?.TryBoxToResult(unboxedResultType, out cachedResult) == true)
        {
            return (T) cachedResult;
        }

        try
        {
            var result = await target(args);

            if (!result.TryUnboxResult(out cachedResult) || cachedResult is null)
            {
                return result;
            }

            await SetItemAsync(_key, cachedResult);

            return result;
        }
        catch (Exception exception)
        {
            return OnException<T>(eventArgs, exception);
        }
    }

    protected abstract object? GetItem(string key, Type targetType);

    protected abstract Task<object?> GetItemAsync(string key, Type targetType);

    protected abstract void SetItem<T>(string key, T value);

    protected abstract Task SetItemAsync<T>(string key, T value);

    protected virtual T OnException<T>(AspectEventArgs eventArgs, Exception exception) => throw exception;
}
