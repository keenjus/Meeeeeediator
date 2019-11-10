using Meeeeeediator.Core.Attributes;
using Meeeeeediator.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Meeeeeediator.Core.Helpers
{
    public static class QueryHelper
    {
        public static string GetQueryName<T>()
        {
            return GetQueryName(typeof(T));
        }

        public static string GetQueryName(Type queryType)
        {
            return queryType.GetCustomAttribute<QueryAttribute>()?.Name ?? queryType.Name;
        }

        internal static IEnumerable<(Type Interface, Type Implementation)> GetQueryHandlers(Assembly assembly)
        {
            foreach (var type in assembly.GetTypes())
            {
                if (type.IsAbstract || type.IsInterface) continue;

                foreach (var @interface in type.GetInterfaces())
                {
                    if (!@interface.IsGenericType) continue;

                    var queryHandlerType = typeof(IQueryHandler<,>);
                    var typeDefinition = @interface.GetGenericTypeDefinition();
                    if (@typeDefinition != queryHandlerType) continue;

                    yield return (@interface, type);
                }
            }
        }

        internal static IDictionary<string, Type> GetQueryTypes(Assembly assembly)
        {
            var queryTypeDictionary = new Dictionary<string, Type>();

            foreach (var type in assembly.GetTypes().Where(x => x.IsPublic && !x.IsAbstract && !x.IsInterface))
            {
                // Make sure the found type implements IQuery<>
                var queryInterfaces = type.GetInterfaces().Where(x => x.IsGenericType).Select(x => x.GetGenericTypeDefinition());
                if (queryInterfaces.Any(i => i == typeof(IQuery<>)))
                {
                    string name = GetQueryName(type);
                    queryTypeDictionary.Add(name, type);
                }
            }

            return queryTypeDictionary;
        }
    }
}
