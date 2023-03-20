using CacheAspectUniversalWrapper.Model;
using Microsoft.AspNetCore.Mvc;

namespace CacheAspectUniversalWrapper.Extensions;

public static class EitherExtensions
{
    public static ActionResult<T> HandleFailuresOrOk<T>(
        this Either<ActionResult, T> result)
    {
        return result.IsRight ? result.Right : result.Left;
    }

    public static async Task<ActionResult<T>> HandleFailuresOrOk<T>(
        this Task<Either<ActionResult, T>> task)
    {
        var result = await task;

        return result.IsRight ? new ActionResult<T>(result.Right) : result.Left;
    }
}
