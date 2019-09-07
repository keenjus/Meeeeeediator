using Meeeeeediator.Core;
using Meeeeeediator.Core.Interfaces;
using System.Threading.Tasks;

namespace Meeeeeediator.Application.Behaviors
{
    public class TestBehavior<TQuery, TReturn> : IBehavior<TQuery, TReturn> where TQuery : IQuery<TReturn>
    {
        public async Task<TReturn> HandleAsync(TQuery query, QueryHandlerDelegate<TReturn> next)
        {
            return await next();
        }
    }
}
