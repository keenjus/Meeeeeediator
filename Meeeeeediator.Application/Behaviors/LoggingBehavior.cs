using Meeeeeediator.Core;
using Meeeeeediator.Core.Interfaces;
using System.Threading.Tasks;

namespace Meeeeeediator.Application.Behaviors
{
    public class LoggingBehavior<TQuery, TReturn> : IBehavior<TQuery, TReturn> where TQuery : IQuery<TReturn>
    {
        public async Task<TReturn> HandleAsync(TQuery query, QueryHandlerDelegate<TReturn> next)
        {
            System.Console.WriteLine($"Executing query \"{query.GetType().Name}\"");

            var response = await next();

            System.Console.WriteLine($"Finished query \"{query.GetType().Name}\"");

            return response;
        }
    }
}
