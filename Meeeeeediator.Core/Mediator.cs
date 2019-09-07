using Meeeeeediator.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meeeeeediator.Core
{
    public delegate Task<TReturn> QueryHandlerDelegate<TReturn>();

    public class Mediator : IMediator
    {
        private readonly IServiceProvider _services;
        private readonly IDictionary<string, Type> _queryTypeDictionary;

        public Mediator(IServiceProvider services, IDictionary<string, Type> queryTypeDictionary)
        {
            _services = services;
            _queryTypeDictionary = queryTypeDictionary;
        }

        public async Task<TReturn> SendAsync<TReturn>(IQuery<TReturn> query)
        {
            var queryType = query.GetType();

            var handler = GetHandler(queryType, typeof(TReturn));
            var handlerDelegate = new QueryHandlerDelegate<TReturn>(() => (Task<TReturn>)GetHandlerResponse(handler, queryType, query));

            var behaviors = GetBehaviors<TReturn>();

            return await behaviors
                // Reverse so the order of registering the behaviors makes sense
                .Reverse()
                .Aggregate(handlerDelegate, (next, pipeline) => () => pipeline.HandleAsync(query, next))();
        }

        public async Task<object> SendAsync(object query)
        {
            var queryType = query.GetType();

            var queryInterface = queryType.GetInterfaces()[0];
            var queryReturnType = queryInterface.GetGenericArguments()[0];

            var handler = GetHandler(queryType, queryReturnType);
            var handlerDelegate = new QueryHandlerDelegate<object>(async () => {
                var task = (Task)GetHandlerResponse(handler, queryType, query);
                await task.ConfigureAwait(false);
                return (object)((dynamic)task).Result;
            });

            return await handlerDelegate();
        }

        public Task<object> SendAsync(string name, string query)
        {
            if (!_queryTypeDictionary.TryGetValue(name, out var queryType))
            {
                throw new InvalidOperationException($"Invalid query \"{name}\"");
            }

            var actualQuery = JsonConvert.DeserializeObject(query, queryType);
            return SendAsync(actualQuery);
        }

        private ICollection<IBehavior<IQuery<TReturn>, TReturn>> GetBehaviors<TReturn>()
        {
            return _services.GetService<IEnumerable<IBehavior<IQuery<TReturn>, TReturn>>>().ToList();
        }

        private object GetHandler(Type queryType, Type returnType)
        {
            var handlerType = typeof(IQueryHandler<,>).MakeGenericType(queryType, returnType);
            return _services.GetRequiredService(handlerType);
        }

        private static object GetHandlerResponse(object handler, Type queryType, object query)
        {
            // This seems very hacky, probably overcomplicating it?
            var method = handler.GetType().GetMethod("HandleAsync", new[] { queryType });
            return method.Invoke(handler, new[] { query });
        }
    }
}
