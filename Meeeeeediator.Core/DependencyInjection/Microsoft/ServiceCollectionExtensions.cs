using Meeeeeediator.Core.Helpers;
using Meeeeeediator.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Meeeeeediator.Core.DependencyInjection.Microsoft
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMediator(this IServiceCollection services, Assembly assembly)
        {
            var queryTypeDictionary = QueryHelper.GetQueryTypes(assembly);

            services.AddScoped<IMediator, Mediator>(sp => new Mediator(new MicrosoftDependencyInjectionResolver(sp), queryTypeDictionary));
            services.AddQueryHandlers(assembly);

            return services;
        }

        private static IServiceCollection AddQueryHandlers(this IServiceCollection services, Assembly assembly)
        {
            foreach (var (Interface, Implementation) in QueryHelper.GetQueryHandlers(assembly))
            {
                services.AddScoped(Interface, Implementation);
            }

            return services;
        }
    }
}
