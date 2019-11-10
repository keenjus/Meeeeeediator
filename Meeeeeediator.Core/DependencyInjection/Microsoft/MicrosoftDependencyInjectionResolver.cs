using Microsoft.Extensions.DependencyInjection;
using System;

namespace Meeeeeediator.Core.DependencyInjection.Microsoft
{
    public class MicrosoftDependencyInjectionResolver : IServiceResolver
    {
        private readonly IServiceProvider _serviceProvider;

        public MicrosoftDependencyInjectionResolver(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public T Resolve<T>()
        {
            return _serviceProvider.GetRequiredService<T>();
        }

        public object Resolve(Type type)
        {
            return _serviceProvider.GetRequiredService(type);
        }
    }
}
