namespace CacheAspectUniversalWrapper.Extensions;

public static class TypeExtensions
{
    /// <summary>
    /// Returns the <see cref="Type.Name"/> property
    /// without including generic type information.
    /// </summary>
    public static string GetNameWithoutGenericArity(this Type t)
    {
        var name = t.Name;
        var index = name.IndexOf('`');

        return index == -1 ? name : name[..index];
    }

    /// <summary>
    /// Get a list of types that describes the path needed to
    /// reach an unboxed 'result' type in the type's generic
    /// arguments e.g. the right of an Either, or the result of a Task.
    /// </summary>
    /// <remarks>
    /// We currently only support 'result' types (e.g. Either, Task
    /// or ActionResult), although it may be useful to handle other
    /// types such as Lists, Dictionary, etc, in the future.
    /// </remarks>
    public static List<Type> GetUnboxedResultTypePath(this Type type)
    {
        var path = new List<Type> {type};

        while (true)
        {
            var currentType = path.Last();

            if (!currentType.IsConstructedGenericType)
            {
                return path;
            }

            // Potentially unsafe to use just the name for this,
            // but realistically, we're never going to create
            // other types with these names.
            switch (currentType.GetNameWithoutGenericArity())
            {
                case "Task":
                case "ActionResult":
                    path.Add(currentType.GenericTypeArguments[0]);
                    continue;

                case "Either":
                    path.Add(currentType.GenericTypeArguments[1]);
                    continue;
            }

            return path;
        }
    }
}
