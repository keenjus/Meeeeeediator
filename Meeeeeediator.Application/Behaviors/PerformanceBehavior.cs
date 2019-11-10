using Meeeeeediator.Core.Delegates;
using Meeeeeediator.Core.Helpers;
using Meeeeeediator.Core.Interfaces;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Meeeeeediator.Application.Behaviors
{
    public class PerformanceBehavior<TQuery, TReturn> : IBehavior<TQuery, TReturn> where TQuery : IQuery<TReturn>
    {
        public async Task<TReturn> HandleAsync(TQuery query, QueryHandlerDelegate<TReturn> next)
        {
            var sw = Stopwatch.StartNew();

            var response = await next();

            sw.Stop();

            Console.WriteLine($"Query \"{QueryHelper.GetQueryName<TQuery>()}\" took {sw.ElapsedMilliseconds}ms");

            return response;
        }
    }
}
