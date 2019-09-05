﻿using Microsoft.Extensions.DependencyInjection;
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

            var handlerType = typeof(IQueryHandler<,>).MakeGenericType(queryType, typeof(TReturn));

            // There has to be a better way to do this, right? Right?!
            var handler = _services.GetRequiredService(handlerType);
            var method = handler.GetType().GetMethod("HandleAsync", new[] { queryType });
            var invoke = method.Invoke(handler, new[] { query });

            return await (Task<TReturn>)invoke;
        }
    }

    public interface IMediator
    {
        Task<TReturn> SendAsync<TReturn>(IQuery<TReturn> query);
    }

    public interface IQuery<TReturn>
    {

    }

    public interface IQueryHandler<TQuery, TReturn> where TQuery : IQuery<TReturn>
    {
        Task<TReturn> HandleAsync(TQuery query);
    }
}
