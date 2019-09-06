using System;

namespace Meeeeeediator.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class QueryAttribute : Attribute
    {
        public string Name { get; set; }
    }
}
