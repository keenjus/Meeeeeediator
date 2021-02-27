using Meeeeeediator.Core.Delegates;
using Meeeeeediator.Core.DependencyInjection;
using Meeeeeediator.Core.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Meeeeeediator.Core
{
    public class Mediator : IMediator
    {
        private readonly IServiceResolver _services;
        private readonly IDictionary<string, Type> _queryTypeDictionary;

        private static readonly MethodInfo _sendAsyncMethod = typeof(Mediator)
            .GetMethods().Single(m => m.Name == nameof(SendAsync) && m.IsGenericMethodDefinition);

        public Mediator(IServiceResolver services, IDictionary<string, Type> queryTypeDictionary)
        {
            _services = services;
            _queryTypeDictionary = queryTypeDictionary;
        }

        public async Task<TReturn> SendAsync<TReturn>(IQuery<TReturn> query)
        {
            var queryType = query.GetType();

            var handler = GetHandler(queryType, typeof(TReturn));
            var handlerDelegate = GetHandlerDelegate<TReturn>(handler, queryType, query);
            var behaviors = GetBehaviors<TReturn>(queryType);

            return await behaviors
                // Reverse so the order of registering the behaviors makes sense
                .Reverse()
                .Aggregate(handlerDelegate, (next, pipeline) => () => pipeline.HandleAsync(query, next))()
                .ConfigureAwait(false);
        }


        public async Task<object> SendAsync(string name, string query)
        {
            if (!_queryTypeDictionary.TryGetValue(name, out var queryType))
            {
                throw new InvalidOperationException($"Invalid query \"{name}\"");
            }

            return await SendAsync(JsonConvert.DeserializeObject(query, queryType), queryType).ConfigureAwait(false);
        }

        private async Task<object> SendAsync(object query, Type queryType)
        {
            var queryInterface = GetQueryInterface(queryType);
            var queryReturnType = queryInterface.GetGenericArguments()[0];

            var method = _sendAsyncMethod.MakeGenericMethod(queryReturnType);
            var task = (Task)method.Invoke(this, new[] { query });

            await task.ConfigureAwait(false);

            return (object)((dynamic)task).Result;
        }

        private ICollection<IBehavior<IQuery<TReturn>, TReturn>> GetBehaviors<TReturn>(Type queryType)
        {
            return _services.Resolve<IEnumerable<IBehavior<IQuery<TReturn>, TReturn>>>().ToList();
        }

        private object GetHandler(Type queryType, Type returnType)
        {
            var handlerType = typeof(IQueryHandler<,>).MakeGenericType(queryType, returnType);
            return _services.Resolve(handlerType);
        }

        private static Type GetQueryInterface(Type queryType)
        {
            return queryType.GetInterfaces().Single(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQuery<>));
        }

        private static QueryHandlerDelegate<TReturn> GetHandlerDelegate<TReturn>(object handler, Type queryType, object query)
        {
            // This seems very hacky, probably overcomplicating it?
            var method = handler.GetType()
                .GetMethod(nameof(IQueryHandler<IQuery<object>, object>.HandleAsync), new[] { queryType });

            return new QueryHandlerDelegate<TReturn>(() => (Task<TReturn>)method.Invoke(handler, new[] { query }));
        }
    }
}
