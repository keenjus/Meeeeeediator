﻿using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Meeeeeediator.Core
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMediator(this IServiceCollection services)
        {
            return services.AddScoped<IMediator, Mediator>();
        }

        public static IServiceCollection AddQueryHandlers(this IServiceCollection services, Assembly assembly)
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
