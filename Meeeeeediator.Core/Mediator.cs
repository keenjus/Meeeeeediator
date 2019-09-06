using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Meeeeeediator.Core.Interfaces;
using Newtonsoft.Json;

namespace Meeeeeediator.Core
{
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
            var task = (Task<TReturn>)GetHandlerValue(handler, queryType, query);

            return await task;
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

        public async Task<object> SendAsync(object query)
        {
            var queryType = query.GetType();

            var queryInterface = queryType.GetInterfaces()[0];
            var queryReturnType = queryInterface.GetGenericArguments()[0];

            var handler = GetHandler(queryType, queryReturnType);
            var task = (Task)GetHandlerValue(handler, queryType, query);

            await task.ConfigureAwait(false);

            return (object)((dynamic)task).Result;
        }

        private static object GetHandlerValue(object handler, Type queryType, object query)
        {
            // This seems very hacky, probably overcomplicating it?
            var method = handler.GetType().GetMethod("HandleAsync", new[] { queryType });
            return method.Invoke(handler, new[] { query });
        }

        private object GetHandler(Type queryType, Type returnType)
        {
            var handlerType = typeof(IQueryHandler<,>).MakeGenericType(queryType, returnType);
            return _services.GetRequiredService(handlerType);
        }
    }
}
