using Meeeeeediator.Core.Delegates;
using Meeeeeediator.Core.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;

namespace Meeeeeediator.Application.Behaviors
{
    public class CachingBehavior<TQuery, TReturn> : IBehavior<TQuery, TReturn> where TQuery : IQuery<TReturn>
    {
        private readonly IMemoryCache _cache;

        public CachingBehavior(IMemoryCache cache)
        {
            _cache = cache;
        }

        public async Task<TReturn> HandleAsync(TQuery query, QueryHandlerDelegate<TReturn> next)
        {
            if (query is ICacheable cacheable)
            {
                if (_cache.TryGetValue(cacheable.CacheKey, out object cachedResponse))
                {
                    return (TReturn)cachedResponse;
                }

                var response = await next();

                _cache.Set(cacheable.CacheKey, response);

                return response;
            }

            return await next();
        }
    }
}
