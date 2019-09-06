using Meeeeeediator.Core;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Meeeeeediator.Core.Interfaces;

namespace Meeeeeediator.Client.CLI
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Wait for API to initialize");

            Thread.Sleep(5000);

            #region Dependency Injection
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddHttpClient("MediatorApi", client =>
            {
                client.BaseAddress = new Uri("http://localhost:5000");
            });

            serviceCollection.AddSingleton<IProxyMediator, ProxyMediator>(sp => new ProxyMediator(sp.GetRequiredService<IHttpClientFactory>().CreateClient("MediatorApi")));

            var serviceProvider = serviceCollection.BuildServiceProvider();
            #endregion

            var proxyMediator = serviceProvider.GetRequiredService<IProxyMediator>();

            Console.WriteLine(await proxyMediator.SendAsync(new Application.Queries.EchoQuery() { Message = "Hello World" }));
            Console.WriteLine(await proxyMediator.SendAsync(new Application.EchoQuery() { Message = "is this uppercase" }));

            Console.ReadLine();
        }
    }
}
