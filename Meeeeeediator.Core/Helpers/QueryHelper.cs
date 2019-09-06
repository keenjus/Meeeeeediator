using System;
using System.Reflection;
using Meeeeeediator.Core.Attributes;

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
    }
}
