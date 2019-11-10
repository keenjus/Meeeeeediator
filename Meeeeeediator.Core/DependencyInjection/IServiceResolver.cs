using System;

namespace Meeeeeediator.Core.DependencyInjection
{
    public interface IServiceResolver
    {
        T Resolve<T>();
        object Resolve(Type type);
    }
}
