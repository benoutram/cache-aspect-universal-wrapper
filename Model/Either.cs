namespace CacheAspectUniversalWrapper.Model;

public class Either<TL, TR>
{
    private readonly TL _left;
    private readonly TR _right;

    public Either(TL left)
    {
        _left = left;
        IsLeft = true;
    }

    public Either(TR right)
    {
        _right = right;
        IsLeft = false;
    }

    public bool IsLeft { get; }

    public bool IsRight => !IsLeft;

    public TL Left => IsLeft ? _left : throw new ArgumentException("Calling Left on a Right");

    public TR Right => !IsLeft ? _right : throw new ArgumentException("Calling Right on a Left");

    private Either<TL, T> Map<T>(Func<TR, T> func) =>
        IsLeft ? new Either<TL, T>(Left) : new Either<TL, T>(func.Invoke(Right));

    public Either<TL, T> OnSuccess<T>(Func<TR, T> func) => Map(func);

    public async Task<Either<TL, T>> OnSuccess<T>(Func<TR, Task<Either<TL, T>>> func)
    {
        if (IsLeft)
        {
            return _left;
        }

        return await func.Invoke(_right);
    }

    public Either<TL, T> OnSuccess<T>(Func<T> func) => Map(_ => func.Invoke());

    public Either<TL, T> OnSuccess<T>(Func<TR, Either<TL, T>> func) => IsLeft ? Left : func.Invoke(Right);

    public Either<TL, TR> OrElse(Func<TR> func) => IsLeft ? func() : Right;

    public Either<TL, TR> OrElse(Func<TL, TR> func) => IsLeft ? func(Left) : Right;

    public T Fold<T>(Func<TL, T> leftFunc, Func<TR, T> rightFunc) => IsRight ? rightFunc(Right) : leftFunc(Left);

    public T FoldLeft<T>(Func<TL, T> leftFunc, T defaultValue) => IsLeft ? leftFunc(Left) : defaultValue;

    public T FoldRight<T>(Func<TR, T> rightFunc, T defaultValue) => IsRight ? rightFunc(Right) : defaultValue;

    public static implicit operator Either<TL, TR>(TL left) => new(left);

    public static implicit operator Either<TL, TR>(TR right) => new(right);
}
