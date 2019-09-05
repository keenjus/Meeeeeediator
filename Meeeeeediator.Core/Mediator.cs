using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Meeeeeediator.Core
{
    public class Mediator : IMediator
    {
        private readonly IServiceProvider _services;

        public Mediator(IServiceProvider services)
        {
            _services = services;
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
            throw new NotImplementedException();
        }

        public async Task<object> SendAsync(object query)
        {
            var queryType = query.GetType();

            var queryInterface = queryType.GetInterfaces()[0];
            var queryReturnType = queryInterface.GetGenericArguments()[0];

            var handler = GetHandler(queryType, queryReturnType);
            var task = (Task)GetHandlerValue(handler, queryType, query);

            await task.ConfigureAwait(false);

            return (object) ((dynamic) task).Result;
        }

        private static object GetHandlerValue(object handler, Type queryType, object query)
        {
            var method = handler.GetType().GetMethod("HandleAsync", new[] { queryType });
            return method.Invoke(handler, new[] { query });
        }


        private object GetHandler(Type queryType, Type returnType)
        {
            var handlerType = typeof(IQueryHandler<,>).MakeGenericType(queryType, returnType);
            return _services.GetRequiredService(handlerType);
        }
    }

    public interface IMediator
    {
        Task<TReturn> SendAsync<TReturn>(IQuery<TReturn> query);

        Task<object> SendAsync(string name, string query);

        Task<object> SendAsync(object query);

    }

    public interface IQuery<TReturn>
    {

    }

    public interface IQueryHandler<in TQuery, TReturn> where TQuery : IQuery<TReturn>
    {
        Task<TReturn> HandleAsync(TQuery query);
    }
}
