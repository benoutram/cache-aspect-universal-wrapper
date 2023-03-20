using System.Reflection;
using AspectInjector.Broker;
using Aspects.Universal.Aspects;

namespace CacheAspectUniversalWrapper.Cache;

[Aspect(Scope.Global)]
public class CacheAspect : BaseUniversalWrapperAspect
{
    [Advice(Kind.Around)]
    public object Handle(
        [Argument(Source.Instance)] object instance,
        [Argument(Source.Type)] Type type,
        [Argument(Source.Method)] MethodBase method,
        [Argument(Source.Target)] Func<object[], object> target,
        [Argument(Source.Name)] string name,
        [Argument(Source.Arguments)] object[] args,
        [Argument(Source.ReturnType)] Type returnType,
        [Argument(Source.Injections)] Attribute[] triggers)
    {
        if (typeof(void) == returnType)
        {
            throw new ArgumentException("Method return type cannot be void");
        }

        if (typeof(Task).IsAssignableFrom(returnType))
        {
            if (!returnType.IsConstructedGenericType)
            {
                throw new ArgumentException("Method return type cannot be Task. Consider using Task<TResult>");
            }
        }

        return BaseHandle(instance, type, method, target, name, args, returnType, triggers);
    }
}
