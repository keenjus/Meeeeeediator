using Meeeeeediator.Core.Delegates;
using Meeeeeediator.Core.Helpers;
using Meeeeeediator.Core.Interfaces;
using System;
using System.Threading.Tasks;

namespace Meeeeeediator.Application.Behaviors
{
    public class LoggingBehavior<TQuery, TReturn> : IBehavior<TQuery, TReturn> where TQuery : IQuery<TReturn>
    {
        public async Task<TReturn> HandleAsync(TQuery query, QueryHandlerDelegate<TReturn> next)
        {
            string queryName = QueryHelper.GetQueryName(query.GetType());

            Console.WriteLine($"Executing query \"{queryName}\"");

            var response = await next();

            Console.WriteLine($"Finished query \"{queryName}\"");

            return response;
        }
    }
}
