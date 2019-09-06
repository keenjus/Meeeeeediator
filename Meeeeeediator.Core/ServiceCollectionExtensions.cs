using Meeeeeediator.Core.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Meeeeeediator.Core.Interfaces;

namespace Meeeeeediator.Core
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMediator(this IServiceCollection services, Assembly assembly)
        {
            var queryTypeDictionary = new Dictionary<string, Type>();

            foreach (var type in assembly.GetTypes().Where(x => x.IsPublic && !x.IsAbstract && !x.IsInterface))
            {
                // Make sure the found type implements IQuery<>
                var queryInterfaces = type.GetInterfaces().Where(x => x.IsGenericType).Select(x => x.GetGenericTypeDefinition());
                if (queryInterfaces.Any(x => x == typeof(IQuery<>)))
                {
                    string name = QueryHelper.GetQueryName(type);
                    queryTypeDictionary.Add(name, type);
                }
            }

            services.AddScoped<IMediator, Mediator>(sp => new Mediator(sp, queryTypeDictionary));
            services.AddQueryHandlers(assembly);

            return services;
        }

        private static IServiceCollection AddQueryHandlers(this IServiceCollection services, Assembly assembly)
        {
            foreach (var type in assembly.GetTypes())
            {
                if (type.IsAbstract || type.IsInterface) continue;

                var interfaces = type.GetInterfaces();

                foreach (var @interface in interfaces)
                {
                    if (!(@interface.IsGenericType)) continue;

                    var queryHandlerType = typeof(IQueryHandler<,>);
                    var typeDefinition = @interface.GetGenericTypeDefinition();
                    if (@typeDefinition == queryHandlerType)
                    {
                        var genericTypes = @interface.GetGenericArguments();
                        services.AddScoped(queryHandlerType.MakeGenericType(genericTypes), type);
                    }
                }
            }

            return services;
        }
    }
}
